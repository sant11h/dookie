using Dookie.Core;
using Dookie.Core.Network;
using Microsoft.Xna.Framework;

namespace Dookie.DesktopGL;

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