using SDL2;
using SharpDX.DirectInput;
using System.Diagnostics;
using static SDL2.SDL;

namespace pixelholdersPlatformer.classes.managers;

public class InputManager
{
    public InputManager()
    {
    }

    public List<InputTypes> GetInputs(Gamepad gamepad)
    {
        List<InputTypes> keysPressed = new List<InputTypes>();

        int numKeys;
        IntPtr keyStatePtr = SDL_GetKeyboardState(out numKeys);
        byte[] keyState = new byte[numKeys];
        System.Runtime.InteropServices.Marshal.Copy(keyStatePtr, keyState, 0, numKeys);

        // Keyboard inputs
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_A] != 0) {keysPressed.Add(InputTypes.PlayerLeft);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_D] != 0) {keysPressed.Add(InputTypes.PlayerRight);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_SPACE] != 0) {keysPressed.Add(InputTypes.PlayerJump);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_K] != 0) {keysPressed.Add(InputTypes.PlayerAttack);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_ESCAPE] != 0) {keysPressed.Add(InputTypes.Quit);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_R] != 0) {keysPressed.Add(InputTypes.ResetPlayerPos);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_F] != 0) {keysPressed.Add(InputTypes.CameraRenderMode);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_UP] != 0) {keysPressed.Add(InputTypes.CameraUp);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_DOWN] != 0) {keysPressed.Add(InputTypes.CameraDown);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_LEFT] != 0) {keysPressed.Add(InputTypes.CameraLeft);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_RIGHT] != 0) {keysPressed.Add(InputTypes.CameraRight);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_KP_PLUS] != 0) {keysPressed.Add(InputTypes.CameraZoomIn);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_RIGHTBRACKET] != 0) {keysPressed.Add(InputTypes.CameraZoomIn);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_KP_MINUS] != 0) {keysPressed.Add(InputTypes.CameraZoomOut);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_SLASH] != 0) {keysPressed.Add(InputTypes.CameraZoomOut);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_KP_0] != 0) {keysPressed.Add(InputTypes.CameraSetZoom2);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_0] != 0) {keysPressed.Add(InputTypes.CameraSetZoom2);}
        if (keyState[(int)SDL_Scancode.SDL_SCANCODE_C] != 0) {keysPressed.Add(InputTypes.CameraCenter);}

        if(SDL_NumJoysticks() < 1) return keysPressed;
        gamepad.Joystick.Poll();
        var state = gamepad.Joystick.GetCurrentState();

        float normalizedX = ((state.X - 0) / 65535.0f) * 2 - 1;
        float normalizedCameraX = ((state.RotationX - 0) / 65535.0f) * 2 - 1;
        float normalizedCameraY = ((state.RotationY - 0) / 65535.0f) * 2 - 1;

        // left stick input
        if (normalizedX < -0.1) keysPressed.Add(InputTypes.PlayerLeft);
        if (normalizedX < -0.8) keysPressed.Add(InputTypes.PlayerLeft);
        if (normalizedX > 0.1) keysPressed.Add(InputTypes.PlayerRight);
        if (normalizedX > 0.8) keysPressed.Add(InputTypes.PlayerRight);
        if (state.Buttons[0]) keysPressed.Add(InputTypes.PlayerJump); // Cross
        if (state.Buttons[1]) keysPressed.Add(InputTypes.Quit); // Circle
        if (state.Buttons[2]) keysPressed.Add(InputTypes.CameraRenderMode); // Square
        if (state.Buttons[3]) keysPressed.Add(InputTypes.ResetPlayerPos); // Triangle
        if (state.Buttons[5]) keysPressed.Add(InputTypes.CameraZoomIn); // Right shoulder
        if (state.Buttons[4]) keysPressed.Add(InputTypes.CameraZoomOut); // Left shoulder
        // right stick input
        if (normalizedCameraX < -0.1) keysPressed.Add(InputTypes.CameraLeft);
        if (normalizedCameraX < -0.8) keysPressed.Add(InputTypes.CameraLeft);
        if (normalizedCameraX > 0.1) keysPressed.Add(InputTypes.CameraRight);
        if (normalizedCameraX > 0.8) keysPressed.Add(InputTypes.CameraRight);
        if (normalizedCameraY < -0.1) keysPressed.Add(InputTypes.CameraUp);
        if (normalizedCameraY < -0.8) keysPressed.Add(InputTypes.CameraUp);
        if (normalizedCameraY > 0.1) keysPressed.Add(InputTypes.CameraDown);
        if (normalizedCameraY > 0.8) keysPressed.Add(InputTypes.CameraDown);
        return keysPressed;
    }
}

public enum InputTypes
{
    PlayerLeft,
    PlayerRight,
    PlayerJump,
    PlayerAttack,
    Quit,
    CameraRenderMode,
    CameraUp,
    CameraDown,
    CameraLeft,
    CameraRight,
    CameraZoomIn,
    CameraZoomOut,
    CameraSetZoom2,
    CameraCenter,
    ResetPlayerPos
}