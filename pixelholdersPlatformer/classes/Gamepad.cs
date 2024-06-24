using SharpDX.DirectInput;

namespace pixelholdersPlatformer.classes;

public class Gamepad
{
    private DirectInput _directInput;
    public Joystick Joystick;

    public Gamepad()
    {
        _directInput = new DirectInput();
        InitializeGamepad();
    }

    private void InitializeGamepad()
    {
        var joystickGuid = Guid.Empty;

        foreach (var deviceInstance in _directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
        {
            joystickGuid = deviceInstance.InstanceGuid;
        }

        if (joystickGuid == Guid.Empty)
        {
            foreach (var deviceInstance in _directInput.GetDevices(DeviceType.Joystick,
                         DeviceEnumerationFlags.AllDevices))
            {
                joystickGuid = deviceInstance.InstanceGuid;
            }
        }

        Joystick = new Joystick(_directInput, joystickGuid);
        Joystick.Properties.BufferSize = 128;
        Joystick.Acquire();
    }

    public void ProcessInput()
    {
        Joystick.Poll();
        var state = Joystick.GetCurrentState();

        if (state.Buttons[0])
        {
            Console.WriteLine($"Cross pressed {state.Buttons[0]}");
        }

        if (state.Buttons[1])
        {
            Console.WriteLine($"Circle pressed {state.Buttons[1]}");
        }

        if (state.Buttons[2])
        {
            Console.WriteLine($"Square pressed {state.Buttons[2]}");
        }

        if (state.Buttons[3])
        {
            Console.WriteLine($"Triangle pressed {state.Buttons[3]}");
        }

        if (state.Buttons[4])
        {
            Console.WriteLine($"Left Shoulder pressed {state.Buttons[4]}");
        }

        if (state.Buttons[5])
        {
            Console.WriteLine($"Right Shoulder pressed {state.Buttons[5]}");
        }

        if (state.Buttons[6])
        {
            Console.WriteLine($"Select pressed {state.Buttons[6]}");
        }

        if (state.Buttons[7])
        {
            Console.WriteLine($"Start pressed {state.Buttons[7]}");
        }

        if (state.Buttons[8])
        {
            Console.WriteLine($"L3 pressed {state.Buttons[8]}");
        }

        if (state.Buttons[9])
        {
            Console.WriteLine($"R3 pressed {state.Buttons[9]}");
        }


        float normalizedX = ((state.X - 0) / 65023.0f) * 2 - 1;
        float normalizedY = ((state.Y - 0) / 65279.0f) * 2 - 1;

        // Console.WriteLine($"Normalized X: {normalizedX}");
        // Console.WriteLine($"Normalized Y: {normalizedY}");

        // TODO implement movement
        // _playerX += (int)(normalizedX * _speed);
        // Console.WriteLine($"Player X: {_playerX}");
        // _playerY += (int)(normalizedY * _speed);
        // Console.WriteLine($"Player Y: {_playerY}");
    }

    public void Update()
    {
        Joystick.Poll();
        var datas = Joystick.GetBufferedData();
        foreach (var state in datas)
        {
            Console.WriteLine(state);
        }
    }
}