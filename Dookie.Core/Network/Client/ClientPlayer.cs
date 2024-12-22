using LiteNetLib;
using Microsoft.Xna.Framework;

namespace Dookie.Core.Network;

public class ClientPlayer : BasePlayer
{
    private PlayerInputPacket _nextCommand;
    private readonly ClientNetworkManager _clientLogic;
    private readonly ClientPlayerManager _playerManager;
    private readonly RingBuffer<PlayerInputPacket> _predictionPlayerStates;
    private ServerState _lastServerState;
    private const int MaxStoredCommands = 60;
    private bool _firstStateReceived;
    private int _updateCount;

    public Vector2 LastPosition { get; private set; }
    public float LastRotation { get; private set; }

    public int StoredCommands => _predictionPlayerStates.Count;

    public ClientPlayer(ClientNetworkManager clientLogic, ClientPlayerManager manager, string name, byte id) : base(manager,
        name, id)
    {
        _playerManager = manager;
        _predictionPlayerStates = new RingBuffer<PlayerInputPacket>(MaxStoredCommands);
        _clientLogic = clientLogic;
    }

    public void ReceiveServerState(ServerState serverState, PlayerState ourState)
    {
        if (!_firstStateReceived)
        {
            if (serverState.LastProcessedCommand == 0)
                return;
            _firstStateReceived = true;
        }

        if (serverState.Tick == _lastServerState.Tick ||
            serverState.LastProcessedCommand == _lastServerState.LastProcessedCommand)
            return;

        _lastServerState = serverState;

        //sync
        position = ourState.Position;
        rotation = ourState.Rotation;
        if (_predictionPlayerStates.Count == 0)
            return;

        var lastProcessedCommand = serverState.LastProcessedCommand;
        var diff = NetworkUtils.SeqDiff(lastProcessedCommand, _predictionPlayerStates.First.Id);

        //apply prediction
        if (diff >= 0 && diff < _predictionPlayerStates.Count)
        {
            _predictionPlayerStates.RemoveFromStart(diff + 1);
            foreach (var state in _predictionPlayerStates)
                ApplyInput(state, LogicTimer.FixedDelta);
        }
        else if (diff >= _predictionPlayerStates.Count)
        {
            Console.WriteLine($"Player input lag st: {_predictionPlayerStates.First.Id} ls:{lastProcessedCommand} df:{diff}");
            //lag
            _predictionPlayerStates.FastClear();
            _nextCommand.Id = lastProcessedCommand;
        }
        else
        {
            Console.WriteLine($"[ERR] SP: {serverState.LastProcessedCommand}, OUR: {_predictionPlayerStates.First.Id}, DF:{diff}, STORED: {StoredCommands}");
        }
    }

    public void SetInput(Vector2 velocity, float rotation)
    {
        _nextCommand.Keys = 0;

        if (velocity.X < -0.5f)
            _nextCommand.Keys |= MovementKeys.Left;
        if (velocity.X > 0.5f)
            _nextCommand.Keys |= MovementKeys.Right;
        if (velocity.Y < -0.5f)
            _nextCommand.Keys |= MovementKeys.Up;
        if (velocity.Y > 0.5f)
            _nextCommand.Keys |= MovementKeys.Down;

        _nextCommand.Rotation = rotation;
    }

    public override void Update(float delta)
    {
        LastPosition = position;
        LastRotation = rotation;

        _nextCommand.Id = (ushort)((_nextCommand.Id + 1) % NetworkConstants.MaxGameSequence);
        _nextCommand.ServerTick = _lastServerState.Tick;
        ApplyInput(_nextCommand, delta);
        if (_predictionPlayerStates.IsFull)
        {
            _nextCommand.Id = (ushort)(_lastServerState.LastProcessedCommand + 1);
            _predictionPlayerStates.FastClear();
        }

        _predictionPlayerStates.Add(_nextCommand);

        _updateCount++;
        if (_updateCount == 3)
        {
            _updateCount = 0;
            foreach (var t in _predictionPlayerStates)
                _clientLogic.SendPacketSerializable(PacketType.Movement, t, DeliveryMethod.Unreliable);
        }

        base.Update(delta);
    }
}

  public class RemotePlayer : BasePlayer
    {
        private readonly RingBuffer<PlayerState> buffer = new(30);
        private float receivedTime;
        private float timer;
        private const float BufferTime = 0.1f; //100 milliseconds

        public RemotePlayer(ClientPlayerManager manager, string name, PlayerJoinedPacket playerJoinedPacket)
            : base(manager, name, playerJoinedPacket.InitialPlayerState.Id)
        {
            position = playerJoinedPacket.InitialPlayerState.Position;
            rotation = playerJoinedPacket.InitialPlayerState.Rotation;
            buffer.Add(playerJoinedPacket.InitialPlayerState);
        }

        public override void Spawn(Vector2 position)
        {
            buffer.FastClear();
            base.Spawn(position);
        }

        public void UpdatePosition(float delta)
        {
            if (receivedTime < BufferTime || buffer.Count < 2)
                return;
            var dataA = buffer[0];
            var dataB = buffer[1];
            
            float lerpTime = NetworkUtils.SeqDiff(dataB.Tick, dataA.Tick)*LogicTimer.FixedDelta;
            float t = timer / lerpTime;
            position = Vector2.Lerp(dataA.Position, dataB.Position, t);
            rotation = MathHelper.Lerp(dataA.Rotation, dataB.Rotation, t);
            timer += delta;
            if (timer > lerpTime)
            {
                receivedTime -= lerpTime;
                buffer.RemoveFromStart(1);
                timer -= lerpTime;
            }
        }

        public void OnPlayerState(PlayerState state)
        {
            //old command
            var diff = NetworkUtils.SeqDiff(state.Tick, buffer.Last.Tick);
            if (diff <= 0)
                return;

            receivedTime += diff * LogicTimer.FixedDelta;
            if (buffer.IsFull)
            {
                Console.WriteLine("[C] Remote: Something happened");
                // Lag?
                receivedTime = 0f;
                buffer.FastClear();
            }
            buffer.Add(state);
        }
    }