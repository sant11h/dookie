using Dookie.Core;
using Dookie.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dookie.DesktopGL;

public class GameLoop : DookieGame
{
    private GameObject javoGameObject;


    protected override void LoadContent()
    {
        var texture2D = Content.Load<Texture2D>("Images/javo");

        javoGameObject = new GameObject();
        var renderer = new Renderer(texture2D);
        var movement = new JavoMovement(InputManager);

        javoGameObject.AddComponent(renderer);
        javoGameObject.AddComponent(movement);
    }

    protected override void Update(GameTime gameTime)
    {
        InputManager.Update(this);

        if (InputManager.KeyPressed(Keys.Escape))
        {
            Exit();
        }

        if (InputManager.KeyPressed(Keys.F1))
        {
            Engine.ToggleFullScreen();
        }

        javoGameObject.Tick(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Engine.SpriteBatch.Begin();

        javoGameObject.Draw(Engine.SpriteBatch);

        Engine.SpriteBatch.End();

        base.Draw(gameTime);
    }
}