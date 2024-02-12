using Microsoft.Xna.Framework;

namespace Dookie.Core.Components;

public class Transform : Component
{
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public float Scale { get; set; }
}