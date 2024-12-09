using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dookie.Core.Components;

public class GameObject : ITickable, IDrawable
{
    public Transform Transform { get; } = new();
    private List<Component> Components { get; } = [];
    private List<IDrawable> DrawableComponents { get; } = [];
    private List<ITickable> TickableComponents { get; } = [];

    public void AddComponent(Component component)
    {
        component.GameObject = this;
        component.Transform = this.Transform;
        this.Components.Add(component);

        if (component is IDrawable drawableComponent)
        {
            this.DrawableComponents.Add(drawableComponent);
        }

        if (component is ITickable tickableComponent)
        {
            this.TickableComponents.Add(tickableComponent);
        }
    }

    public T? GetComponentOrDefault<T>()
        where T : Component
    {
        foreach (var component in this.Components)
        {
            if (component is T genericComponent)
            {
                return genericComponent;
            }
        }

        return null;
    }

    public T GetComponent<T>()
        where T : Component
    {
        var component = this.GetComponentOrDefault<T>();
        if (component is null)
        {
            throw new InvalidOperationException($"The component {typeof(T).Name} is not attached to this GameObject.");
        }

        return component;
    }

    public void Tick(GameTime gameTime)
    {
        foreach (var tickableComponent in this.TickableComponents)
        {
            tickableComponent.Tick(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var drawableComponent in this.DrawableComponents)
        {
            drawableComponent.Draw(spriteBatch);
        }
    }
}