namespace Dookie.Core.Network;

public class ClientPlayerManager : BasePlayerManager
{
    public readonly Dictionary<byte, PlayerHandler> Players;
    private readonly ClientNetworkManager _clientLogic;
    private ClientPlayer _clientPlayer;

    public ClientPlayer? CurrentPlayer => _clientPlayer;
    public override int Count => Players.Count;

    public ClientPlayerManager(ClientNetworkManager clientLogic)
    {
        _clientLogic = clientLogic;
        Players = new Dictionary<byte, PlayerHandler>();
    }
        
    public override IEnumerator<BasePlayer> GetEnumerator()
    {
        foreach (var ph in Players)
            yield return ph.Value.Player;
    }

    public void ApplyServerState(ref ServerState serverState)
    {
        for (int i = 0; i < serverState.PlayerStatesCount; i++)
        {
            var state = serverState.PlayerStates[i];
            if(!Players.TryGetValue(state.Id, out var handler))
                return;

            if (handler.Player == _clientPlayer)
            {
                _clientPlayer.ReceiveServerState(serverState, state);
            }
            else
            {
                var rp = (RemotePlayer)handler.Player;
                rp.OnPlayerState(state);
            }
        }
    }

    // public override void OnShoot(BasePlayer from, Vector2 to, BasePlayer hit)
    // {
    //     if(from == _clientPlayer)
    //         _clientLogic.SpawnShoot(from.Position, to);
    // }

    public BasePlayer GetById(byte id)
    {
        return Players.TryGetValue(id, out var ph) ? ph.Player : null;
    }

    public BasePlayer RemovePlayer(byte id)
    {
        if (Players.Remove(id, out var handler))
        {
            handler.View.Destroy();
        }
        
        return handler.Player;
    }

    public override void LogicUpdate()
    {
        foreach (var kv in Players)
            kv.Value.Update(LogicTimer.FixedDelta);
    }

    public void AddClientPlayer(ClientPlayer player, ClientPlayerComponent component)
    {
        _clientPlayer = player;
        Players.Add(player.Id, new PlayerHandler(player, component));
    }
        
    public void AddPlayer(RemotePlayer player, RemotePlayerComponent component)
    {
        Players.Add(player.Id, new PlayerHandler(player, component));
    }

    public void Clear()
    {
        foreach (var p in Players.Values)
            p.View.Destroy();
        Players.Clear();
    }
}