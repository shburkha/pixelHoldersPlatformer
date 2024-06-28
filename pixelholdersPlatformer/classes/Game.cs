using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.classes.managers;
using pixelholdersPlatformer.gameObjects;
using SDL2;
using System.Diagnostics;
using static SDL2.SDL;

namespace pixelholdersPlatformer;

public class Game
{

    private List<GameObject> gameObjects;

    Stopwatch stopwatch = new Stopwatch();
    private bool _quit;

    private Player _player;

    private double _frameInterval;
    private RenderManager _renderManager;
    private InputManager _inputManager;
    public Game()
    {
        _renderManager = new RenderManager();
        _inputManager = new InputManager();
        gameObjects = new List<GameObject>();
        _quit = false;

        GameObject object1 = new GameObject(51.2f, 51, 2, 2);
        GameObject object2 = new GameObject(51, 51, 2, 2);
        GameObject object3 = new GameObject(45, 51, 2, 2);
        GameObject object4 = new GameObject(45.7f, 51, 2, 2);
        GameObject object5 = new GameObject(0, 0, 100, 100);
        _player = new Player(55, 55, 1, 1);

        gameObjects.Add(object1);
        gameObjects.Add(object2);
        gameObjects.Add(object3);
        gameObjects.Add(object4);
        gameObjects.Add(object5);
        gameObjects.Add(_player);

        _renderManager.SetGameObjects(gameObjects);

        SDL_DisplayMode _displayMode;
        SDL_GetCurrentDisplayMode(0, out _displayMode);

        int _refreshRate = _displayMode.refresh_rate;
        double targetFPS = _refreshRate * 1.0d;
        _frameInterval = 1000d / targetFPS;
        stopwatch.Start();


    }


    public void StartGame()
    {
        SDL_Event e;

        while (SDL_PollEvent(out e) != 0 || !_quit)
        {
            if (e.type == SDL_EventType.SDL_QUIT)
            {
                _quit = true;
            }

            double timeElapsed = (double)stopwatch.ElapsedMilliseconds;
            if (timeElapsed > _frameInterval)
            {
                processInput();
                update();
                render();
                stopwatch.Restart();
            }
           
        }
    }

    private void processInput()
    {
        List<InputTypes> inputs = _inputManager.GetInputs();

        for (int i = 0; i < inputs.Count; i++)
        {
            switch (inputs[i])
            {
                case InputTypes.PlayerLeft:
                    _player.MovePlayerX(-1);
                    break;
                case InputTypes.PlayerRight:
                    _player.MovePlayerX(1);

                    break;
                case InputTypes.PlayerJump:
                    _player.MovePlayerY(-1);
                    break;
                case InputTypes.Quit:
                    _quit = true;
                    break;
                case InputTypes.CameraRenderMode:
                    _renderManager.SwitchRenderMode();
                    break;
                case InputTypes.CameraUp:
                    _renderManager.MoveCamera(0, -1);
                    break;
                case InputTypes.CameraDown:
                    _renderManager.MoveCamera(0, 1);
                    break;
                case InputTypes.CameraLeft:
                    _renderManager.MoveCamera(-1, 0);
                    break;
                case InputTypes.CameraRight:
                    _renderManager.MoveCamera(1, 0);
                    break;
                case InputTypes.CameraZoomIn:
                    _renderManager.Zoom(-1);
                    break;
                case InputTypes.CameraZoomOut:
                    _renderManager.Zoom(1);
                    break;
                case InputTypes.CameraSetZoom2:
                    _renderManager.SetZoomLevel(2);
                    break;
                case InputTypes.CameraCenter:
                    _renderManager.CenterCameraAroundPlayer();
                    break;

            }
        }
    }

    private void update()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.Update();
        }
    }

    private void render()
    {
        _renderManager.CenterCameraAroundPlayer();

        _renderManager.WipeScreen();
        _renderManager.RenderGameObjects();
    }
}