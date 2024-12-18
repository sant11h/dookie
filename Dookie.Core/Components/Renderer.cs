using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dookie.Core;

public class Renderer(Texture2D texture) : Component, IDrawable
{
    public readonly Texture2D Texture = texture;

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            this.Transform.Position,
            this.Transform.Body,
            this.Transform.Color);
    }
}