﻿using System.Net;
using System.Net.Sockets;
using Dookie.Core;
using Dookie.Core.Network;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dookie.DesktopGL;

public class ClientNetworkManager : INetEventListener
{
    private GameLoop game;

    public readonly NetManager Client;
    private NetDataWriter dataWriter;
    private NetPacketProcessor packetProcessor;
    
    private Action<DisconnectInfo>? onDisconnected;
    private string userName;
    private ServerState cachedServerState;
    // private ShootPacket cachedShootData;
    private ushort lastServerTick;
    private NetPeer server;
    private ClientPlayerManager playerManager;
    private int ping;
    public static LogicTimer LogicTimer { get; private set; }

    public ClientNetworkManager(GameLoop game)
    {
        this.game = game;
        cachedServerState = new ServerState();
        userName = Environment.MachineName + " " + Random.Shared.Next(100000);
        LogicTimer = new LogicTimer(OnLogicUpdate);
        dataWriter = new NetDataWriter();
        playerManager = new ClientPlayerManager();
        // shootsPool = new GamePool<ShootEffect>(ShootEffectContructor, 100);
        packetProcessor = new NetPacketProcessor();
        packetProcessor.RegisterNestedType((w, v) => w.Put(v), reader => reader.GetVector2());
        packetProcessor.RegisterNestedType<PlayerState>();
        packetProcessor.SubscribeReusable<PlayerJoinedPacket>(OnPlayerJoined);
        packetProcessor.SubscribeReusable<JoinAcceptPacket>(OnJoinAccept);
        packetProcessor.SubscribeReusable<PlayerLeavedPacket>(OnPlayerLeaved);
        
        Client = new NetManager(this)
        {
            AutoRecycle = true,
        };

        Client.Start();
    }

    public void Connect(string host, Action<DisconnectInfo> onDisconnect)
    {
        this.onDisconnected = onDisconnect;
        Client.Connect(
            host,
            NetworkConstants.ServerPort,
            NetworkConstants.ServerKey /* text key or NetDataWriter */);
    }
    
    public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : class, new()
    {
        if (server == null)
            return;
        
        dataWriter.Reset();
        dataWriter.Put((byte) PacketType.Serialized);
        packetProcessor.Write(dataWriter, packet);
        server.Send(dataWriter, deliveryMethod);
    }
    
    public void SendPacketSerializable<T>(PacketType type, T packet, DeliveryMethod deliveryMethod) where T : INetSerializable
    {
        if (server == null)
            return;
        dataWriter.Reset();
        dataWriter.Put((byte)type);
        packet.Serialize(dataWriter);
        server.Send(dataWriter, deliveryMethod);
    }
    
    
    private void OnLogicUpdate()
    {
        playerManager.LogicUpdate();
    }

    public void Update(GameTime gameTime)
    {
        Client.PollEvents();
        LogicTimer.Update();
        /*if (playerManager.CurrentPlayer != null)
            Console.WriteLine(string.Format(
                $"LastServerTick: {lastServerTick}\n" +
                $"StoredCommands: {playerManager.CurrentPlayer.StoredCommands}\n" +
                $"Ping: {ping}"));
        else
            Console.WriteLine("Disconnected");*/
    }

    private void OnDestroy()
    {
        Client.Stop();
    }

    private void OnPlayerJoined(PlayerJoinedPacket packet)
    {
        Console.WriteLine($"[C] Player joined: {packet.UserName}");
        var remotePlayer = new RemotePlayer(playerManager, packet.UserName, packet);
        var remotePlayerComponent = InstantiatePlayerGameObject(remotePlayer);
        playerManager.AddPlayer(remotePlayer, remotePlayerComponent);
    }
    
    private void OnPlayerLeaved(PlayerLeavedPacket packet)
    {
        var player = playerManager.RemovePlayer(packet.Id);
        if(player != null) Console.WriteLine($"[C] Player leaved: {player.Name}");
    }

    private void OnJoinAccept(JoinAcceptPacket packet)
    {
        Console.WriteLine("[C] Join accept. Received player id: " + packet.Id);
        lastServerTick = packet.ServerTick;
        var clientPlayer = new ClientPlayer(this, playerManager, userName, packet.Id);
        var clientPlayerComponent = InstantiatePlayerGameObject(clientPlayer);
        playerManager.AddClientPlayer(clientPlayer, clientPlayerComponent);
    }

    private ClientPlayerComponent InstantiatePlayerGameObject(ClientPlayer clientPlayer)
    {
        // paddles
        const int paddleHeight = 30;
        const int paddleWidth = 150;
        var paddleTexture = new Texture2D(game.Engine.Graphics.GraphicsDevice, 1, 1);
        paddleTexture.SetData([Color.White]);
        
        // first paddle
        var firstPaddlePositionX = game.Engine.Graphics.PreferredBackBufferWidth / 2f - paddleWidth / 2f;
        var firstPaddlePositionY = game.Engine.Graphics.PreferredBackBufferHeight - paddleHeight;
        var firstPaddleGameObject = new GameObject
        {
            Transform =
            {
                Position = new Vector2(0, 0),
            }
        };
        
        firstPaddleGameObject.AddComponent(
            new Renderer(
                paddleTexture,
                new Rectangle((int)firstPaddlePositionX, firstPaddlePositionY, paddleWidth, paddleHeight),
                Color.Plum));

        var clientPlayerComponent = new ClientPlayerComponent(clientPlayer,
            game.Services.GetService<InputManager>());
        firstPaddleGameObject.AddComponent(clientPlayerComponent);
        
        game.GameObjects.Add(firstPaddleGameObject);

        return clientPlayerComponent;
    }
    
    private RemotePlayerComponent InstantiatePlayerGameObject(RemotePlayer remotePlayer)
    {
        // paddles
        const int paddleHeight = 30;
        const int paddleWidth = 150;
        var paddleTexture = new Texture2D(game.Engine.Graphics.GraphicsDevice, 1, 1);
        paddleTexture.SetData([Color.White]);
        
        // first paddle
        var firstPaddlePositionX = game.Engine.Graphics.PreferredBackBufferWidth / 2f - paddleWidth / 2f;
        var firstPaddlePositionY = game.Engine.Graphics.PreferredBackBufferHeight - paddleHeight;
        var firstPaddleGameObject = new GameObject
        {
            Transform =
            {
                Position = new Vector2(0, 0),
            }
        };
        
        firstPaddleGameObject.AddComponent(
            new Renderer(
                paddleTexture,
                new Rectangle((int)firstPaddlePositionX, firstPaddlePositionY, paddleWidth, paddleHeight),
                Color.Plum));

        var clientPlayerComponent = new RemotePlayerComponent(remotePlayer);
        firstPaddleGameObject.AddComponent(clientPlayerComponent);
        
        game.GameObjects.Add(firstPaddleGameObject);

        return clientPlayerComponent;
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Console.WriteLine("Connected to server: " + peer);

        server = peer;
        SendPacket(new JoinPacket {UserName = userName}, DeliveryMethod.ReliableOrdered);
        LogicTimer.Start();
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        playerManager.Clear();
        server = null;
        LogicTimer.Stop();
        
        Console.WriteLine("Disconnected from server: " + disconnectInfo.Reason);
        
        if (this.onDisconnected == null) return;
        this.onDisconnected(disconnectInfo);
        this.onDisconnected = null;
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        Console.Error.WriteLine("{0}", socketError);
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        var packetType = reader.GetByte();
        if (packetType >= NetworkConstants.PacketTypesCount)
            throw new InvalidOperationException("Invalid packet type");
        
        var pt = (PacketType)packetType;
        switch (pt)
        {
            case PacketType.Spawn:
                break;
            
            case PacketType.ServerState:
                cachedServerState.Deserialize(reader);
                OnServerState();
                break;
            
            case PacketType.Serialized:
                packetProcessor.ReadAllPackets(reader);
                break;

            case PacketType.Movement:
                break;
            
            default:
                throw new InvalidOperationException("Invalid packet type");
        }
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        this.ping = latency;
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        request.Reject();
    }
    
    private void OnServerState()
    {
        //skip duplicate or old because we received that packet unreliably
        if (NetworkUtils.SeqDiff(cachedServerState.Tick, lastServerTick) <= 0)
            return;
        lastServerTick = cachedServerState.Tick;
        playerManager.ApplyServerState(ref cachedServerState);
    }
}