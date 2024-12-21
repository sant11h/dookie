using Dookie.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dookie.DesktopGL;

public class BallMovement(
    InputManager inputManager,
    GraphicsDeviceManager graphics) : Component, ITickable
{
    public Vector2 Direction { get; set; } = VectorUtilities.RandomDirection();
    
    public void Tick(GameTime gameTime)
    {
        const float ballSpeed = 0.5f;
        var renderer = this.GameObject.GetComponent<Renderer>();
        this.Transform.Position += this.Direction * ballSpeed * gameTime.ElapsedGameTime.Milliseconds;
        renderer.Body = new Rectangle((int)this.Transform.Position.X,(int)this.Transform.Position.Y, renderer.Body.Width, renderer.Body.Height);

        if (IsAtTop() || IsAtBottom(renderer.Body))
        {
           this.Direction = new Vector2(this.Direction.X,this.Direction.Y * -1);
        }
        
        if (IsAtLeft() || IsAtRight(renderer.Body))
        {
           this.Direction = new Vector2(this.Direction.X * -1,this.Direction.Y);
        }

        if (inputManager.KeyPressed(Keys.F))
        {
            this.Transform.Position = new Vector2(
                (float)graphics.GraphicsDevice.Viewport.Width / 2,
                (float)graphics.GraphicsDevice.Viewport.Height / 2);
           this.Direction = VectorUtilities.RandomDirection();
        }
    }

    private bool IsAtTop() => this.Transform.Position.Y < 0;
    private bool IsAtBottom(Rectangle body) => this.Transform.Position.Y + body.Height  > graphics.PreferredBackBufferHeight;
    private bool IsAtLeft() => this.Transform.Position.X < 0;
    private bool IsAtRight(Rectangle body) => this.Transform.Position.X + body.Width > graphics.PreferredBackBufferWidth;
}