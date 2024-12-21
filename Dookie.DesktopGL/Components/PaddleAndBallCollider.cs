using Dookie.Core;
using Microsoft.Xna.Framework;

namespace Dookie.DesktopGL;

public class PaddleAndBallCollider(GameObject ballGameObject) : Component, ITickable
{
    public void Tick(GameTime gameTime)
    {
        var paddleRenderer = this.GameObject.GetComponent<Renderer>();
        var ballRenderer = ballGameObject.GetComponent<Renderer>();
        var ballMovement = ballGameObject.GetComponent<BallMovement>();

        if (paddleRenderer.Body.Intersects(ballRenderer.Body))
        {
            ballMovement.Direction = 
                new Vector2(
                    ballMovement.Direction.X,
                    ballMovement.Direction.Y * -1);
        }
    }
}