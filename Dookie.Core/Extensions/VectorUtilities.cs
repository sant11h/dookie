using Microsoft.Xna.Framework;

namespace Dookie.Core;

public static class VectorUtilities
{
    public static Vector2 RandomDirection()
    {
        var random = new Random();
        return new Vector2(random.Next(2) * 2 - 1, random.Next(2) * 2 - 1);
    }
}