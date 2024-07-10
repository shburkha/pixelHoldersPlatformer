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

    private Player _player;

    private double _deltaT;

    private double _frameInterval;
    private RenderManager _renderManager;
    private InputManager _inputManager;
    private CollisionManager _collisionManager;
    private AnimationManager _animationManager;

    Gamepad _gamepad;
    TileMapManager _tileMapManager;

    public Game()
    {
        _renderManager = new RenderManager();
        _animationManager = new AnimationManager(_renderManager);
        _inputManager = new InputManager();
        _collisionManager = new CollisionManager();

        gameObjects = new List<GameObject>();
        _quit = false;

        _gamepad = new Gamepad();
        _tileMapManager = new TileMapManager();

        GameObject border = new GameObject(0, 0, 100, 100);
        GameObject platform = new GameObject(50, 50, 10, 2);
        platform.AddComponent(new MovableComponent(platform));
        platform.AddComponent(new PhysicsComponent(platform));
        platform.AddComponent(new CollisionComponent(platform));
        GameObject wall = new GameObject(49.5f, 50, 1, 7);
        wall.AddComponent(new MovableComponent(wall));
        wall.AddComponent(new PhysicsComponent(wall));
        wall.AddComponent(new CollisionComponent(wall));
        GameObject wall2 = new GameObject(59.5f, 50, 1, 7);
        wall2.AddComponent(new MovableComponent(wall2));
        wall2.AddComponent(new PhysicsComponent(wall2));
        wall2.AddComponent(new CollisionComponent(wall2));
        GameObject ceiling = new GameObject(0, 38, 100, 2);
        ceiling.AddComponent(new MovableComponent(ceiling));
        ceiling.AddComponent(new PhysicsComponent(ceiling));
        ceiling.AddComponent(new CollisionComponent(ceiling));
        _player = new Player(50, 28, 2.6f, 2);


        gameObjects.Add(border);
        //gameObjects.Add(platform);
        gameObjects.Add(ceiling);
        //gameObjects.Add(wall);
        //gameObjects.Add(wall2);
        gameObjects.Add(_player);

        _renderManager.SetGameObjects(gameObjects);
        _collisionManager.SetGameObjects(gameObjects);
        _animationManager.SetGameObjects(gameObjects);

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
                _deltaT = timeElapsed;
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
                    _player.MovePlayerX(-0.5f);
                    break;
                case InputTypes.PlayerRight:
                    _player.MovePlayerX(0.5f);
                    break;
                case InputTypes.PlayerJump:
                    if (((PhysicsComponent)_player.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).Velocity.Y == 0)
                    {
                        _player.MovePlayerY(-1);
                    }
                    break;
                case InputTypes.Quit:
                    _quit = true;
                    break;
                case InputTypes.ResetPlayerPos:
                    _player.ResetPlayerPosition();
                    break;
                case InputTypes.CameraRenderMode:
                    _renderManager.SwitchRenderMode();
                    break;
                case InputTypes.CameraUp:
                    _renderManager.MoveCamera(0, -0.5f);
                    break;
                case InputTypes.CameraDown:
                    _renderManager.MoveCamera(0, 0.5f);
                    break;
                case InputTypes.CameraLeft:
                    _renderManager.MoveCamera(-1, 0);
                    break;
                case InputTypes.CameraRight:
                    _renderManager.MoveCamera(0.5f, 0);
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
                    _renderManager.Zoom(1);
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
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetType().Name == "Player")
            {
                ((Player)gameObject).SetDeltaTime(_deltaT/1000d);
            }
            gameObject.Update();
        }
        _collisionManager.HandleCollision();
        _animationManager.AnimateObjects();
    }

    private void Render()
    {
        _renderManager.CenterCameraAroundPlayer();
        _renderManager.WipeScreen();
        _renderManager.RenderGameObjects();
    }
}