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

        if (joystickGuid == Guid.Empty) return; // this fixes crash when no gamepad is connected
        Joystick = new Joystick(_directInput, joystickGuid);
        Joystick.Properties.BufferSize = 128;
        Joystick.Acquire();
    }
}