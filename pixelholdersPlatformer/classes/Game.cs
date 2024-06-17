namespace pixelholdersPlatformer.classes;

public class Game
{
    Gamepad _gamepad;

    public Game()
    {
        _gamepad = new Gamepad();
    }

    public void StartGame()
    {
        while (true)
        {
            ProcessInput();
            Update();
            Render();
        }
    }

    private void ProcessInput()
    {
        _gamepad.Joystick.Poll();
        var state = _gamepad.Joystick.GetCurrentState();

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

    }

    private void Update()
    {

    }

    private void Render()
    {

    }
}