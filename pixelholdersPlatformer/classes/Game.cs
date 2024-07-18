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

    private List<GameObject> _collidableTiles;
    private List<SpecialTile> _specialTiles;

    private List<Cannon> _cannons;

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

        _renderManager.SetMapData(_tileMapManager.GetMapData());

        _collidableTiles = _tileMapManager.GetEnvironmentCollidables();
        foreach (var box in _collidableTiles)
        {
            //box.AddComponent(new MovableComponent(box));
            box.AddComponent(new PhysicsComponent(box));
            box.AddComponent(new CollisionComponent(box));
            gameObjects.Add(box);
        }

        _specialTiles = _tileMapManager.GetSpecialTiles();
        foreach (var box in _specialTiles)
        {
            //box.AddComponent(new MovableComponent(box));
            box.AddComponent(new PhysicsComponent(box));
            box.AddComponent(new CollisionComponent(box));
            gameObjects.Add(box);
        }

        _player = new Player(5, 5, 1, 1);

        gameObjects.Add(_player);

        _cannons = new List<Cannon>();
        _cannons.Add(new Cannon(7, 3, Direction.Right));

        foreach (var cannon in _cannons)
        {
            gameObjects.Add(cannon);
        }

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
        List<InputTypes> inputs = _inputManager.GetInputs(_gamepad);

        foreach (var input in inputs)
        {
            switch (input)
            {
                case InputTypes.PlayerLeft:
                    _player.MovePlayerX(-0.5f);
                    break;
                case InputTypes.PlayerRight:
                    _player.MovePlayerX(0.5f);
                    break;
                case InputTypes.PlayerJump:
                    if (((PhysicsComponent)_player.GetComponent(Component.Physics)).Velocity.Y == 0)
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
    }

    private void Update()
    {
        List<GameObject> objectsToAdd = new List<GameObject>();
        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject is Player)
            {
                ((Player)gameObject).SetDeltaTime(_deltaT/1000d);
            }
            else if (gameObject is Cannon)
            {
                ((Cannon)gameObject).SetDeltaTime(_deltaT/1000d);
                if (((Cannon)gameObject).CanShoot())
                {
                    objectsToAdd.Add(((Cannon)gameObject).Shoot());
                }
            }
            else if (gameObject is Cannonball)
            {
                ((Cannonball)gameObject).SetDeltaTime(_deltaT / 1000d);
                if (gameObject.CoordX > _renderManager.GetMapWidth() || gameObject.CoordX < 0)
                {
                    objectsToRemove.Add(gameObject);
                }
            }
            gameObject.Update();
        }
        _collisionManager.HandleCollision();
        _animationManager.AnimateObjects();
        foreach (GameObject gameObject in objectsToAdd)
        {
            gameObjects.Add(gameObject);
        }
        foreach (GameObject gameObject in objectsToRemove)
        {
            gameObjects.Remove(gameObject);
        }
    }

    private void Render()
    {
        _renderManager.CenterCameraAroundPlayer();
        _renderManager.WipeScreen();
        _renderManager.RenderGameObjects();
    }
}