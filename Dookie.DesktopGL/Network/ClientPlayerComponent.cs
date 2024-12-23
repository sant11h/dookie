using Dookie.Core;
using Dookie.Core.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dookie.DesktopGL;

public class ClientPlayerComponent(ClientPlayer clientPlayer, InputManager inputManager) : Component, ITickableComponent
{
    public void Destroy()
    {
        // destroy game object
    }

    public void Tick(GameTime gameTime)
    {
        var speedUp = inputManager.KeyHeld(Keys.Space);
        
        var velocity = Vector2.Zero;
        if (inputManager.KeyHeld(Keys.Left))
        {
            velocity = new Vector2(-1, 0);
        }

        if (inputManager.KeyHeld(Keys.Right))
        {
            velocity = new Vector2(1, 0);
        }

        clientPlayer.SetInput(velocity, speedUp);

        var lerpT = ClientNetworkManager.LogicTimer.LerpAlpha;
        
        this.Transform.Position = Vector2.Lerp(clientPlayer.LastPosition, clientPlayer.Position, lerpT);
        // renderer.Body = new Rectangle((int)this.Transform.Position.X,(int)this.Transform.Position.Y, renderer.Body.Width,renderer.Body.Height);
    }
}