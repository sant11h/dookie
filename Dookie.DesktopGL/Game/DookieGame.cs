using Dookie.Core;
using Microsoft.Xna.Framework;

namespace Dookie.DesktopGL;

public class DookieGame : Game
{
    internal Engine Engine;
    internal InputManager InputManager;
    
    public DookieGame()
    {
        Engine = this.InitializeEngine();
        InputManager = this.Services.GetService<InputManager>();
    }
}