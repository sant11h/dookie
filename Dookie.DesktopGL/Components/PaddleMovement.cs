using Dookie.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dookie.DesktopGL;

public class PaddleMovement(
    InputManager inputManager,
    GraphicsDeviceManager graphics,
    Keys leftKey,
    Keys rightKey,
    Keys speedUpKey) : Component, ITickable
{
    public void Tick(GameTime gameTime)
    {
        var speed = 400f;

        if (inputManager.KeyHeld(speedUpKey))
        {
            speed *= 2;
        }
        else
        {
            speed = 400f;
        }

        var updatedSpeed = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        var renderer = this.GameObject.GetComponent<Renderer>();

        if (inputManager.KeyHeld(leftKey))
        {
            var tempPosition =
                new Vector2(
                    Math.Clamp(this.Transform.Position.X - updatedSpeed, 0,
                        graphics.PreferredBackBufferWidth - renderer.Body.Width), this.Transform.Position.Y);

            this.Transform.Position = tempPosition;
        }

        if (inputManager.KeyHeld(rightKey))
        {
            var tempPosition =
                new Vector2(
                    Math.Clamp(this.Transform.Position.X + updatedSpeed, 0,
                        graphics.PreferredBackBufferWidth - renderer.Body.Width), this.Transform.Position.Y);

            this.Transform.Position = tempPosition;
        }
        
        renderer.Body = new Rectangle((int)this.Transform.Position.X,(int)this.Transform.Position.Y, renderer.Body.Width,renderer.Body.Height);
    }
}