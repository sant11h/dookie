using LiteNetLib;

namespace Dookie.Core.Network;

public class ServerPlayer : BasePlayer
{
    private readonly ServerPlayerManager _playerManager;
    public readonly NetPeer AssociatedPeer;
    public PlayerState NetworkState;
    public ushort LastProcessedCommandId { get; private set; }
        
    public ServerPlayer(ServerPlayerManager playerManager, string name, NetPeer peer) : base(playerManager, name, (byte)peer.Id)
    {
        _playerManager = playerManager;
        peer.Tag = this;
        AssociatedPeer = peer;
        NetworkState = new PlayerState {Id = (byte) peer.Id};
    }

    public override void ApplyInput(PlayerInputPacket command, float delta)
    {
        if (NetworkUtils.SeqDiff(command.Id, LastProcessedCommandId) <= 0)
            return;
        LastProcessedCommandId = command.Id;
        base.ApplyInput(command, delta);
    }

    public override void Update(float delta)
    {
        base.Update(delta);
        NetworkState.Position = position;
        NetworkState.Rotation = rotation;
        NetworkState.Tick = LastProcessedCommandId;
            
        //Draw cross as server player
        // const float sz = 0.1f;
        // Debug.DrawLine(
        //     new Vector2(Position.x - sz, Position.y ),
        //     new Vector2(Position.x + sz, Position.y ), 
        //     Color.white);
        // Debug.DrawLine(
        //     new Vector2(Position.x, Position.y - sz ),
        //     new Vector2(Position.x, Position.y + sz ), 
        //     Color.white);
    }
}