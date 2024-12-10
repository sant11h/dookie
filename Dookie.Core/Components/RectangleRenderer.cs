using Microsoft.Xna.Framework.Graphics;

namespace Dookie.Core;

public class RectangleRenderer(Texture2D texture) : Component, IDrawable
{
    public readonly Texture2D Texture = texture;

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            this.Transform.Body,
            this.Transform.Color);
    }
}