using Dookie.Core;
using Dookie.Core.Network;
using Microsoft.Xna.Framework;

namespace Dookie.DesktopGL;

public class DookieGame : Game
{
    internal readonly Engine Engine;
    internal readonly InputManager InputManager;

    protected DookieGame()
    {
        Engine = this.InitializeEngine();
        InputManager = this.Services.GetService<InputManager>();
    }
}
