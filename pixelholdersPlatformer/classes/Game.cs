using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.classes.managers;
using pixelholdersPlatformer.gameObjects;
using SDL2;
using System.Diagnostics;
using pixelholdersPlatformer.classes;
using static SDL2.SDL;
using TiledCSPlus;
using pixelholdersPlatformer.classes.behaviours;
using pixelholdersPlatformer.classes.states;
using System.Numerics;
using System;

namespace pixelholdersPlatformer;

public class Game
{
    private List<GameObject> gameObjects;

    private List<GameObject> _collidableTiles;
    private List<SpecialTile> _specialTiles;

    private List<Cannon> _cannons;
    private List<Enemy> _enemies;

    Stopwatch _gameStopwatch = new Stopwatch();
    private bool _quit;

    private Player _player;

    private Enemy _testEnemy;
    private Enemy _testEnemy2;


    private double _deltaT;

    private double _frameInterval;
    private RenderManager _renderManager;
    private InputManager _inputManager;
    private CollisionManager _collisionManager;
    private AnimationManager _animationManager;

    private const int _attackCooldown = 500;
    private Stopwatch _attackStopWatch = new Stopwatch();

    private const int _jumpCooldown = 100;
    private Stopwatch _jumpStopWatch = new Stopwatch(); 

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
        _tileMapManager = TileMapManager.Instance;
        TileMapManager.Instance.OnLevelAdvanced += HandleLevelAdvanced;


        _specialTiles = _tileMapManager.GetSpecialTiles();
        foreach (var box in _specialTiles)
        {
            //box.AddComponent(new MovableComponent(box));
            box.AddComponent(new PhysicsComponent(box));
            box.AddComponent(new CollisionComponent(box));
            gameObjects.Add(box);
        }

        //the sizes are important, don't change them please :)

        _cannons = new List<Cannon>();
        _enemies = new List<Enemy>();

        LoadMap();
        
        SDL_DisplayMode _displayMode;
        SDL_GetCurrentDisplayMode(0, out _displayMode);

        int _refreshRate = _displayMode.refresh_rate;
        double targetFPS = _refreshRate * 1.0d;
        _frameInterval = 1000d / targetFPS;

        _gameStopwatch.Start();
        _attackStopWatch.Start();
        _jumpStopWatch.Start();
    }

    public void StartGame()
    {
        SDL_Event e;

        while (SDL_PollEvent(out e) != 0 || !_quit)
        {
            if (e.type == SDL_EventType.SDL_QUIT)
            {
                AudioManager.Instance.Dispose();
                _quit = true;
            }

            double timeElapsed = (double)_gameStopwatch.ElapsedMilliseconds;
            if (timeElapsed > _frameInterval)
            {
                _deltaT = timeElapsed;
                ProcessInput();
                Update();
                Render();
                _gameStopwatch.Restart();
            }
        }
    }

    private void LoadMap()
    {
        //change no. 1
        gameObjects = new List<GameObject>();

        _renderManager.SetMapData(_tileMapManager.GetMapData());
        _collidableTiles = _tileMapManager.GetEnvironmentCollidables();

        foreach (var box in _collidableTiles)
        {
            box.AddComponent(new MovableComponent(box));
            box.AddComponent(new PhysicsComponent(box));
            box.AddComponent(new CollisionComponent(box));
            gameObjects.Add(box);
        }

        _specialTiles = _tileMapManager.GetSpecialTiles();
        foreach (var box in _specialTiles)
        {
            box.AddComponent(new MovableComponent(box));
            box.AddComponent(new PhysicsComponent(box));
            box.AddComponent(new CollisionComponent(box));
            gameObjects.Add(box);
        }

        //change no. 2
        _player = new Player(5, 10, 1, 1);
        gameObjects.Add(_player);
        AddEntities();
        _renderManager.SetGameObjects(gameObjects);
        _collisionManager.SetGameObjects(gameObjects);
        _animationManager.SetGameObjects(gameObjects);

        _player.SetEnemies(gameObjects.FindAll(t => t is Enemy));
    }

    private void AddEntities()
    {
        switch (TileMapManager.Instance.CurrentLevel)
        {
            case 2:


                Enemy kingPig = new Enemy(10, 10, 0.6f, 0.6f);
                kingPig.SetEnemyType("02-King Pig");
                kingPig.AddComponent(new PigBehaviour(kingPig, _player, _collidableTiles));
                _enemies.Add(kingPig);

                _enemies.Add(CreatePig(28, 16));
                _enemies.Add(CreatePig(44, 16));
                _enemies.Add(CreatePig(58, 16));
                _enemies.Add(CreatePig(72, 15));
                _enemies.Add(CreatePig(81, 15));
                break;
            case 3:
                _enemies.Add(CreatePig(30, 15));
                _enemies.Add(CreatePig(45, 15));
                _enemies.Add(CreatePig(111, 17));
                _enemies.Add(CreatePig(121, 16));
                _enemies.Add(CreatePig(139, 14));
                _enemies.Add(CreatePig(147, 14));
                _cannons.Add(new Cannon(79.5f, 10.5f, Direction.Left));
                _cannons.Add(new Cannon(82, 11.5f, Direction.Right));
                _cannons.Add(new Cannon(86, 12.5f, Direction.Right));
                _cannons.Add(new Cannon(90, 13.5f, Direction.Right));
                _cannons.Add(new Cannon(94, 14.5f, Direction.Right));
                _cannons.Add(new Cannon(135.5f, 15.5f, Direction.Left));
                _cannons.Add(new Cannon(154.5f, 14.5f, Direction.Left));
                break;
        }

        foreach (var cannon in _cannons)
        {
            gameObjects.Add(cannon);
        }

        foreach (var enemy in _enemies)
        {
            gameObjects.Add(enemy);
        }
    }

    private Enemy CreatePig(float x, float y)
    {
        Enemy enemy = new Enemy(x, y, 0.5f, 0.5f);
        enemy.AddComponent(new PigBehaviour(enemy, _player, _collidableTiles));
        return enemy;
    }

    private void HandleLevelAdvanced()
    {
        LoadMap();
        
    }


    private PlayerInput _currentPlayerInput = PlayerInput.None;

    private void ProcessInput()
    {
        List<InputTypes> inputs = _inputManager.GetInputs(_gamepad);

        if (!inputs.Contains(InputTypes.PlayerLeft) && !inputs.Contains(InputTypes.PlayerRight)
            || ((PhysicsComponent)_player.GetComponent(Component.Physics)).Velocity.Y != 0) AudioManager.Instance.StopRunning();

        foreach (var input in inputs)
        {
            switch (input)
            {
                case InputTypes.PlayerJump:
                    if (_jumpStopWatch.ElapsedMilliseconds > _jumpCooldown)
                    {
                        _currentPlayerInput = PlayerInput.Jump;
                        _jumpStopWatch.Restart();
                        
                    }
                   
                    break;
                case InputTypes.PlayerLeft:
                    _currentPlayerInput = PlayerInput.Left;
                    break;
                case InputTypes.PlayerRight:
                    _currentPlayerInput = PlayerInput.Right;
                    break;


                case InputTypes.PlayerAttack:
                    if (_attackStopWatch.ElapsedMilliseconds > _attackCooldown)
                    {
                        _currentPlayerInput = PlayerInput.Attack;
                        ((AnimatableComponent)_player.GetComponent(Component.Animatable)).SetAnimationType(AnimationType.Attack, ((AnimatableComponent)_player.GetComponent(Component.Animatable)).isFlipped);
                        // TODO fix! Not always synced with animation
                        AudioManager.Instance.PlaySound("attack");
                        _attackStopWatch.Restart();
                    }

                    break;



                case InputTypes.Quit:
                    AudioManager.Instance.Dispose();
                    _quit = true;
                    break;
                case InputTypes.ResetPlayerPos:
                    _player.ResetPlayerPosition();
                    break;
                case InputTypes.Checkpoint:
                    _player.PlayerToCheckpoint();
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
                case InputTypes.DebugMode:
                    _renderManager.SwitchDebugMode();
                    break;
            }
        }
        _player.HandleInput(_currentPlayerInput);
        _currentPlayerInput = PlayerInput.None;
    }

    private void Update()
    {
        List<GameObject> objectsToAdd = new List<GameObject>();
        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject is Player)
            {
                ((Player)gameObject).SetDeltaTime(_deltaT / 1000d);
            }
            else if (gameObject is Cannon)
            {
                ((Cannon)gameObject).SetDeltaTime(_deltaT / 1000d);
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