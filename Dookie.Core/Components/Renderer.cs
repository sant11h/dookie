using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dookie.Core.Components;

public class Renderer : Component, IDrawable
{
    public Texture2D Texture { get; }

    public Renderer(Texture2D texture)
    {
        this.Texture = texture;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            this.Transform.Position, Color.White);
    }
}