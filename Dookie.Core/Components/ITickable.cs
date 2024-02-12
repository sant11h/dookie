using Microsoft.Xna.Framework;

namespace Dookie.Core.Components;

public interface ITickable
{
    void Tick(GameTime gameTime);
}