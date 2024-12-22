using Dookie.Core;
using Dookie.Core.Network;
using LiteNetLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dookie.DesktopGL;

public class GameLoop : DookieGame
{
    private readonly List<GameObject> gameObjects = [];
    private ServerNetworkManager ServerNetworkManager;
    private ClientNetworkManager ClientNetworkManager;
    

    protected override void LoadContent()
    {
        // TODO: Properly mode to wherever it belongs
        ClientNetworkManager = new ClientNetworkManager(this, gameObjects, Engine.Graphics);
        // start server
        ServerNetworkManager = new ServerNetworkManager();
        ServerNetworkManager.Start();
        
        ClientNetworkManager.Connect("localhost", OnDisconnect);
        
        // background
        const int backgroundWidth = 1; // single pixel width (stretched later)
        const int backgroundHeight = 256; // gradient resolution
        var backgroundTexture = new Texture2D(Engine.Graphics.GraphicsDevice, backgroundWidth, backgroundHeight);

        var colors = new Color[backgroundHeight];
        for (var y = 0; y < backgroundHeight; y++)
        {
            var t = y / (float)(backgroundHeight - 1); // interpolation factor
            colors[y] = Color.Lerp(Color.Blue, Color.Purple, t); // interpolate between colors
        }

        backgroundTexture.SetData(colors);
        var backgroundGameObject = new GameObject();
        backgroundGameObject.AddComponent(
            new Renderer(
                backgroundTexture,
                new Rectangle(0, 0, Engine.Graphics.PreferredBackBufferWidth, Engine.Graphics.PreferredBackBufferHeight)));

        // firstPaddleGameObject.AddComponent(new PaddleMovement(InputManager, Engine.Graphics, Keys.Left, Keys.Right, Keys.Down));
        
        // // second paddle
        // var secondPaddlePositionX = Engine.Graphics.PreferredBackBufferWidth / 2f - paddleWidth / 2f;
        // const int secondPaddlePositionY = 0;
        // var secondPaddleGameObject = new GameObject
        // {
        //     Transform =
        //     {
        //         Position = new Vector2(secondPaddlePositionX , secondPaddlePositionY),
        //     }
        // };
        // secondPaddleGameObject.AddComponent(
        //     new Renderer(
        //         paddleTexture,
        //         new Rectangle((int)secondPaddlePositionX, secondPaddlePositionY, paddleWidth, paddleHeight),
        //         Color.Plum));
        // secondPaddleGameObject.AddComponent(new PaddleMovement(InputManager, Engine.Graphics, Keys.A, Keys.D, Keys.S));
        //
        // // ball
        // const int ballHeight = 20;
        // const int ballWidth = 20;
        // var ballTexture = new Texture2D(Engine.Graphics.GraphicsDevice, 1, 1);
        // ballTexture.SetData([Color.White]);
        //
        // var ballPositionX = Engine.Graphics.PreferredBackBufferWidth / 2f;
        // var ballPositionY = Engine.Graphics.PreferredBackBufferHeight / 2f;
        // var ballGameObject = new GameObject
        // {
        //     Transform =
        //     {
        //         Position = new Vector2(ballPositionX,ballPositionY),
        //     }
        // };
        //
        // ballGameObject.AddComponent(new Renderer(ballTexture, new Rectangle((int)ballPositionX, (int)ballPositionY, ballHeight, ballWidth), Color.Plum));
        // ballGameObject.AddComponent(new BallMovement(InputManager, Engine.Graphics));
        //
        // // add collision between ball and paddles
        // firstPaddleGameObject.AddComponent(new PaddleAndBallCollider(ballGameObject));
        // secondPaddleGameObject.AddComponent(new PaddleAndBallCollider(ballGameObject));
        
        // Add game objects
        gameObjects.Add(backgroundGameObject);
        // gameObjects.Add(secondPaddleGameObject);
        // gameObjects.Add(ballGameObject);
    }

    private void OnDisconnect(DisconnectInfo obj)
    {
        Console.WriteLine(obj.Reason);
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

        ServerNetworkManager.Update();
        ClientNetworkManager.Update(gameTime);

        foreach (var gameObject in gameObjects)
        {
            gameObject.Tick(gameTime);
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        Engine.SpriteBatch.Begin();
        
        foreach (var gameObject in gameObjects)
        {
            gameObject.Draw(Engine.SpriteBatch);
        }

        Engine.SpriteBatch.End();

        base.Draw(gameTime);
    }
}