using SharpDX.DirectInput;

namespace pixelholdersPlatformer.classes;

public class Gamepad
{
    private DirectInput _directInput;
    public Joystick Joystick;
    public Guid JoystickGuid;

    public Gamepad()
    {
        _directInput = new DirectInput();
        JoystickGuid = Guid.Empty;
        InitializeGamepad();
    }

    private void InitializeGamepad()
    {
        foreach (var deviceInstance in _directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
        {
            JoystickGuid = deviceInstance.InstanceGuid;
        }

        if (JoystickGuid == Guid.Empty)
        {
            foreach (var deviceInstance in _directInput.GetDevices(DeviceType.Joystick,
                         DeviceEnumerationFlags.AllDevices))
            {
                JoystickGuid = deviceInstance.InstanceGuid;
            }
        }

        if (JoystickGuid == Guid.Empty) return; // this fixes crash when no gamepad is connected
        Joystick = new Joystick(_directInput, JoystickGuid);
        Joystick.Properties.BufferSize = 128;
        Joystick.Acquire();
    }

}