using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.classes.managers;
using pixelholdersPlatformer.gameObjects;
using SDL2;
using System.Diagnostics;
using pixelholdersPlatformer.classes;
using static SDL2.SDL;
using TiledCSPlus;

namespace pixelholdersPlatformer;

public class Game
{
    private List<GameObject> gameObjects;

    Stopwatch stopwatch = new Stopwatch();
    private bool _quit;


    private double _frameInterval;
    private RenderManager _renderManager;
    private InputManager _inputManager;

    Gamepad _gamepad;
    private TiledMap _map;
    private TiledTileset _decoTileset;
    private TiledTileset _terrainTileset;
    private TiledLayer _myLayer;
    private TiledObject _myObject;

    public Game()
    {
        _renderManager = new RenderManager();
        _inputManager = new InputManager();
        gameObjects = new List<GameObject>();
        _quit = false;

        _gamepad = new Gamepad();
        _map = new TiledMap("assets/test.tmx");
        _decoTileset = new TiledTileset("assets/Decorations.tsx");
        _terrainTileset = new TiledTileset("assets/Terrain.tsx");
        _myLayer = _map.Layers.First();

        GameObject object1 = new GameObject(51.2f, 51, 2, 2);
        object1.AddComponent(new RenderingComponent());
        GameObject object2 = new GameObject(51, 51, 2, 2);
        object2.AddComponent(new RenderingComponent());
        GameObject object3 = new GameObject(45, 51, 2, 2);
        object3.AddComponent(new RenderingComponent());
        GameObject object4 = new GameObject(45.7f, 51, 2, 2);
        object4.AddComponent(new RenderingComponent());
        GameObject object5 = new GameObject(0, 0, 100, 100);
        object5.AddComponent(new RenderingComponent());

        gameObjects.Add(object1);
        gameObjects.Add(object2);
        gameObjects.Add(object3);
        gameObjects.Add(object4);
        gameObjects.Add(object5);

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
                ProcessInput();
                Update();
                Render();
                stopwatch.Restart();
            }
        }
    }

    private void ProcessInput()
    {
        List<InputTypes> inputs = _inputManager.GetInputs();

        for (int i = 0; i < inputs.Count; i++)
        {
            switch (inputs[i])
            {
                case InputTypes.PlayerLeft:
                    //_player.Move('left');
                    break;
                case InputTypes.PlayerRight:
                    //_player.Move('right');
                    break;
                case InputTypes.PlayerJump:
                    //_player.Jump();
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
                    _renderManager.Zoom(-2);
                    break;
                case InputTypes.CameraZoomOut:
                    _renderManager.Zoom(2);
                    break;
            }
        }

        if (_gamepad.Joystick != null)
        {
            List<InputTypes> gamepadInputs = _inputManager.GetGamepadInputs(_gamepad.Joystick);
            foreach (var input in gamepadInputs)
            {
                switch (input)
                {
                    case InputTypes.PlayerLeft:
                        //_player.Move('left');
                        break;
                    case InputTypes.PlayerRight:
                        //_player.Move('right');
                        break;
                    case InputTypes.PlayerJump:
                        //_player.Jump();
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
                        _renderManager.Zoom(-2);
                        break;
                    case InputTypes.CameraZoomOut:
                        _renderManager.Zoom(2);
                        break;
                }
            }
        }
    }

    private void Update()
    {
    }

    private void Render()
    {
        _renderManager.WipeScreen();
        _renderManager.RenderGameObjects();
    }
}