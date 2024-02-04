using Dookie.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dookie.DesktopGL;

public class GameLoop : DookieGame
{
    private Texture2D javoImage;
    private Vector2 javoPosition;

    protected override void LoadContent()
    {
        javoImage = Content.Load<Texture2D>("Images/javo");
    }

    protected override void Update(GameTime gameTime)
    {
        InputManager.Update();

        if (InputManager.KeyPressed(Keys.Escape))
        {
            Exit();
        }

        if (InputManager.KeyPressed(Keys.F1))
        {
            Engine.ToggleFullScreen();
        }

        if (InputManager.MouseLeftButtonHeld())
        {
            javoPosition = new Vector2(
                InputManager.MousePosition.X - (float)javoImage.Width / 2,
                InputManager.MousePosition.Y - (float)javoImage.Height / 2);
        }

        if (InputManager.MouseRightButtonPressed())
        {
            var random = new Random();
            javoPosition = new Vector2(
                random.Next(0, Constants.DefaultWindowWidth - javoImage.Width),
                random.Next(0, Constants.DefaultWindowHeight - javoImage.Height));
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Engine.SpriteBatch.Begin();

        Engine.SpriteBatch.Draw(javoImage, javoPosition, Color.White);

        Engine.SpriteBatch.End();

        base.Draw(gameTime);
    }
}