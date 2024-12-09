using Microsoft.Xna.Framework;

namespace Dookie.Core.Components;

public class JavoMovement : Component, ITickable
{
    private readonly InputManager inputManager;

    public JavoMovement(InputManager inputManager)
    {
        this.inputManager = inputManager;
    }

    public void Tick(GameTime gameTime)
    {
        var renderer = this.GameObject.GetComponent<Renderer>();
        if (inputManager.MouseLeftButtonHeld())
        {
            var tempPosition = new Vector2(
                inputManager.MousePosition.X - (float)renderer.Texture.Width / 2,
                inputManager.MousePosition.Y - (float)renderer.Texture.Height / 2);

            this.Transform.Position = tempPosition;
        }

        if (inputManager.MouseRightButtonPressed())
        {
            var random = new Random();
            var tempPosition = new Vector2(
                random.Next(0, Constants.DefaultWindowWidth - renderer.Texture.Width),
                random.Next(0, Constants.DefaultWindowHeight - renderer.Texture.Height));

            Console.WriteLine($"Right click position {tempPosition.X}, {tempPosition.Y}");
            
            this.Transform.Position = tempPosition;
        }
    }
}