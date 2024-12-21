using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dookie.Core;

public class Renderer(Texture2D texture, Rectangle? body = null, Color? color = null) : Component, IDrawable
{
    public readonly Texture2D Texture = texture;
    public Rectangle Body = body ?? Rectangle.Empty;
    public Color Color = color ?? Color.White;

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            this.Transform.Position,
            Body,
            Color);
    }
}