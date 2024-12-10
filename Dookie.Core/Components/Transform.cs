using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dookie.Core;

public class Transform : Component
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    
    public float Rotation { get; set; }
    
    public Color Color { get; set; } = Color.White;
    
    public Rectangle Body { get; set; }

    public Vector2 Direction { get; set; }
}
