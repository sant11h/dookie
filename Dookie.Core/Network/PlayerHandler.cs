namespace Dookie.Core.Network;

public struct PlayerHandler
{
    public readonly BasePlayer Player;
    public readonly ITickableComponent View;

    public PlayerHandler(BasePlayer player, ITickableComponent view)
    {
        Player = player;
        View = view;
    }

    public void Update(float delta)
    {
        Player.Update(delta);
    }
}