using Dookie.Core;
using Microsoft.Xna.Framework;

namespace Dookie.DesktopGL;

public class PaddleAndBallCollider(Transform ballTransform) : Component, ITickable
{
    public void Tick(GameTime gameTime)
    {
        if (this.Transform.Body.Intersects(ballTransform.Body))
        {
            ballTransform.Direction = new Vector2(ballTransform.Direction.X, ballTransform.Direction.Y * -1);
        }
    }
}