using Microsoft.Xna.Framework;

namespace Dookie.Core;

public interface ITickable
{
    void Tick(GameTime gameTime);
}