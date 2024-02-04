using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dookie.Core;

public class Engine
{
    private Game game;

    public GraphicsDeviceManager Graphics;
    public SpriteBatch SpriteBatch;
    public Point WindowSize;

    public Engine(Game game)
    {
        this.game = game;
        Graphics = new GraphicsDeviceManager(game);
        game.Content.RootDirectory = "Content";
        game.IsMouseVisible = true;

        WindowSize = new Point(Constants.DefaultWindowWidth, Constants.DefaultWindowHeight);
        Graphics.PreferredBackBufferWidth = WindowSize.X;
        Graphics.PreferredBackBufferHeight = WindowSize.Y;
        Graphics.ApplyChanges();
        
        SpriteBatch = new SpriteBatch(game.GraphicsDevice);
        game.ConfigureServices();
    }

    public void ToggleFullScreen()
    {
        if (Graphics.IsFullScreen)
        {
            Graphics.PreferredBackBufferWidth = WindowSize.X;
            Graphics.PreferredBackBufferHeight = WindowSize.Y;
            Graphics.HardwareModeSwitch = false;

            Graphics.IsFullScreen = false;
        }
        else
        {
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.HardwareModeSwitch = false;

            Graphics.IsFullScreen = true;
        }

        Graphics.ApplyChanges();
    }
}