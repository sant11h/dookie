using Dookie.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dookie.DesktopGL;

public class BallMovement(
    InputManager inputManager,
    GraphicsDeviceManager graphics) : Component, ITickable
{
    public void Tick(GameTime gameTime)
    {
        const float ballSpeed = 0.5f;
        this.Transform.Position += this.Transform.Direction * ballSpeed * gameTime.ElapsedGameTime.Milliseconds;
        this.Transform.Body = new Rectangle((int)this.Transform.Position.X,(int)this.Transform.Position.Y, this.Transform.Body.Width, this.Transform.Body.Height);

        if (IsAtTop() || IsAtBottom())
        {
            this.Transform.Direction = new Vector2(this.Transform.Direction.X, this.Transform.Direction.Y * -1);
        }
        
        if (IsAtLeft() || IsAtRight())
        {
            this.Transform.Direction = new Vector2(this.Transform.Direction.X * -1, this.Transform.Direction.Y);
        }

        if (inputManager.KeyPressed(Keys.F))
        {
            this.Transform.Position = new Vector2(
                (float)graphics.GraphicsDevice.Viewport.Width / 2,
                (float)graphics.GraphicsDevice.Viewport.Height / 2);
            this.Transform.Direction = Vector2.Normalize(VectorUtilities.RandomDirection());
        }
    }

    private bool IsAtTop() => this.Transform.Position.Y < 0;
    private bool IsAtBottom() => this.Transform.Position.Y + this.Transform.Body.Height  > graphics.PreferredBackBufferHeight;
    private bool IsAtLeft() => this.Transform.Position.X < 0;
    private bool IsAtRight() => this.Transform.Position.X + this.Transform.Body.Width > graphics.PreferredBackBufferWidth;
}