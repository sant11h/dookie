using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dookie.Core;

public class Transform : Component
{
    public Vector2 Position { get; set; } = Vector2.Zero;

    public float Rotation { get; set; }
}
