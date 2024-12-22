using Microsoft.Xna.Framework;

namespace Dookie.Core.Network;

public class RemotePlayerComponent(RemotePlayer remotePlayer) : Component, ITickableComponent
{
    public void Destroy()
    {
        // destroy game object
    }

    public void Tick(GameTime gameTime)
    {
        remotePlayer.UpdatePosition(gameTime.ElapsedGameTime.Milliseconds);
        this.Transform.Position = remotePlayer.Position;
    }
}