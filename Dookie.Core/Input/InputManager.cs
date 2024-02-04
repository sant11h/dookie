using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dookie.Core;

public class InputManager
{
    private MouseState currentMouseState, previousMouseState;
    private KeyboardState currentKeyboardState, previousKeyboardState;

    public void Update(Game game)
    {
        if (!game.IsActive) return;
        
        previousMouseState = currentMouseState;
        previousKeyboardState = currentKeyboardState;
        currentMouseState = Mouse.GetState();
        currentKeyboardState = Keyboard.GetState();
    }

    public Vector2 MousePosition => new(currentMouseState.X, currentMouseState.Y);

    public bool MouseLeftButtonPressed()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed &&
               previousMouseState.LeftButton == ButtonState.Released;
    }

    public bool MouseRightButtonPressed()
    {
        return currentMouseState.RightButton == ButtonState.Pressed &&
               previousMouseState.RightButton == ButtonState.Released;
    }

    public bool MouseLeftButtonHeld()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed;
    }

    public bool MouseRightButtonHeld()
    {
        return currentMouseState.RightButton == ButtonState.Pressed;
    }

    public bool KeyPressed(Keys k)
    {
        return currentKeyboardState.IsKeyDown(k) && previousKeyboardState.IsKeyUp(k);
    }
}