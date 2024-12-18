﻿using Microsoft.Xna.Framework;

namespace Dookie.Core;

public static class GameExtensions
{
    public static Engine InitializeEngine(this Game game)
    {
        var engine = new Engine(game);
        game.Services.AddService(engine);

        return engine;
    }

    public static void ConfigureServices(this Game game)
    {
        game.Services.AddService(new InputManager());
    }
}