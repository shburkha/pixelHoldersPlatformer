using SDL2;
using static SDL2.SDL;

namespace pixelholdersPlatformer.classes.managers;

public class InputManager
{
    public InputManager()
    {

    }

    public List<InputTypes> GetInputs()
    {
        List<InputTypes> keysPressed = new List<InputTypes>();

        int numKeys;
        IntPtr keyStatePtr = SDL_GetKeyboardState(out numKeys);
        byte[] keyState = new byte[numKeys];
        System.Runtime.InteropServices.Marshal.Copy(keyStatePtr, keyState, 0, numKeys);

        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_A] != 0) { keysPressed.Add(InputTypes.PlayerLeft); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_D] != 0) { keysPressed.Add(InputTypes.PlayerRight); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_SPACE] != 0) { keysPressed.Add(InputTypes.PlayerJump); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_ESCAPE] != 0) { keysPressed.Add(InputTypes.Quit); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_R] != 0) { keysPressed.Add(InputTypes.CameraRenderMode); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_UP] != 0) { keysPressed.Add(InputTypes.CameraUp); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_DOWN] != 0) { keysPressed.Add(InputTypes.CameraDown); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_LEFT] != 0) { keysPressed.Add(InputTypes.CameraLeft); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_RIGHT] != 0) { keysPressed.Add(InputTypes.CameraRight); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_KP_PLUS] != 0) { keysPressed.Add(InputTypes.CameraZoomIn); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_KP_MINUS] != 0) { keysPressed.Add(InputTypes.CameraZoomOut); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_KP_0] != 0) { keysPressed.Add(InputTypes.CameraSetZoom2); }
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_C] != 0) { keysPressed.Add(InputTypes.CameraCenter); }
        return keysPressed;
    }



}


public enum InputTypes 
{ 
    PlayerLeft, PlayerRight, PlayerJump, Quit, CameraRenderMode, CameraUp, CameraDown, CameraLeft, CameraRight, CameraZoomIn, CameraZoomOut, CameraSetZoom2, CameraCenter
}