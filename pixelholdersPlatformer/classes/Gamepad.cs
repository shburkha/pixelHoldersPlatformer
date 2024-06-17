﻿using SharpDX.DirectInput;
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

        if (joystickGuid == Guid.Empty)
        {
            throw new Exception("No joystick or gamepad found.");
        }

        Joystick = new Joystick(_directInput, joystickGuid);
        Joystick.Properties.BufferSize = 128;
        Joystick.Acquire();
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