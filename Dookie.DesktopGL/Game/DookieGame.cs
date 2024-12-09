using Dookie.Core;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Xna.Framework;

namespace Dookie.DesktopGL;

public class DookieGame : Game
{
    internal readonly Engine Engine;
    internal readonly InputManager InputManager;
    internal HubConnection HubConnection;
    internal Vector2 position;

    protected DookieGame()
    {
        Engine = this.InitializeEngine();
        InputManager = this.Services.GetService<InputManager>();

        HubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5070/position").Build();
        HubConnection.On<float, float>("UpdatePosition", (x, y) => 
        {
            position.X = x;
            position.Y = y;
        });

        HubConnection.StartAsync();
    }

    internal void Move(float x, float y)
    {
        HubConnection.InvokeAsync("Move", x, y);
    }
}
