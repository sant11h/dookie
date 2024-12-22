using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dookie.Core.Network;

public interface ITickableComponent : IPlayerView, ITickable;

public class ClientPlayerComponent(ClientPlayer clientPlayer, InputManager inputManager) : Component, ITickableComponent
{
    public void Destroy()
    {
        // destroy game object
    }

    public void Tick(GameTime gameTime)
    {
        var speed = 400f;

        if (inputManager.KeyHeld(Keys.Down))
        {
            speed *= 2;
        }
        else
        {
            speed = 400f;
        }
        
        var updatedSpeed = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        var velocity = Vector2.Zero;
        if (inputManager.KeyHeld(Keys.Left))
        {
            velocity = new Vector2(-1, 0);
        }

        if (inputManager.KeyHeld(Keys.Right))
        {
            velocity = new Vector2(0, 1);
        }

        clientPlayer.SetInput(velocity, 0);

        var lerpT = ClientNetworkManager.LogicTimer.LerpAlpha;
        
        this.Transform.Position = Vector2.Lerp(clientPlayer.LastPosition, clientPlayer.Position, lerpT);
        // renderer.Body = new Rectangle((int)this.Transform.Position.X,(int)this.Transform.Position.Y, renderer.Body.Width,renderer.Body.Height);
    }
}