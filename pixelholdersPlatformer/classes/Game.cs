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
    private UIManager _uiManager;

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

        _uiManager = UIManager.Instance;

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
                _enemies.Add(CreatePig(87, 13));
                _enemies.Add(CreatePig(94, 11));
                _enemies.Add(CreatePig(99, 15));
                _enemies.Add(CreatePig(117, 10));
                _cannons.Add(new Cannon(134.5f, 6.5f, Direction.Left));
                _cannons.Add(new Cannon(139.5f, 5.5f, Direction.Left));
                _cannons.Add(new Cannon(147, 5.5f, Direction.Right));
                _cannons.Add(new Cannon(160, 9.5f, Direction.Right));
                _cannons.Add(new Cannon(170, 12.5f, Direction.Right));
                _cannons.Add(new Cannon(169, 13.5f, Direction.Right));
                _cannons.Add(new Cannon(179, 16.5f, Direction.Right));
                _cannons.Add(new Cannon(178, 17.5f, Direction.Right));
                _cannons.Add(new Cannon(177, 18.5f, Direction.Right));
                _enemies.Add(CreatePig(158, 5));
                _enemies.Add(CreatePig(168, 9));
                _enemies.Add(CreatePig(188, 18));
                _enemies.Add(CreatePig(196, 18));
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
        //the sizes are important, don't change them please :)
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
        if (_uiManager.CurrentScene != Scene.Game)
        {
            SDL_Point mousePos = new SDL_Point();
            var mouseState = SDL_GetMouseState(out mousePos.x, out mousePos.y);

            if (SDL_BUTTON(mouseState) == 1) //true if only left mouse button is pressed
            {
                var clickedElement = _renderManager.GetMouseOverTextElement(_uiManager.GetCurrentSceneTextElements(), mousePos.x, mousePos.y);
                if (clickedElement != null)
                {
                    switch (clickedElement.GetText())
                    {
                        case "Start":
                            _uiManager.ChangeScene(Scene.Game);
                            break;
                        case "Options":
                            _uiManager.ChangeScene(Scene.Settings);
                            break;
                        case "Play Again":
                            _player.ResetPlayerPosition();
                            _player.PlayerHealth = 3;
                            _uiManager.ChangeScene(Scene.Game);
                            TileMapManager.Instance.CurrentLevel = 1;
                            break;
                        case "Main Menu":
                            _uiManager.ChangeScene(Scene.MainMenu);
                            break;
                        case "Off":
                        case "On":
                            _renderManager.ChangeWindowSize();
                            _uiManager.ToggleFullscreenStatus();
                            SDL_Delay(200);
                            break;
                        case "Exit":
                            _quit = true;
                            break;
                    }
                }
            }

            return;
        }

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
        if (_uiManager.CurrentScene != Scene.Game) //don't update game while not in the game scene
        {
            if (!_renderManager.IsCameraReset()) //make sure the camera is in the correct position for displaying menu ui
            {
                _renderManager.ResetCameraPos();
            }
            return;
        }

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
            else if (gameObject is Enemy)
            {
                if ((gameObject as Enemy).EnemyHealth <= 0)
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
        _renderManager.WipeScreen();

        switch (_uiManager.CurrentScene)
        {
            case Scene.Game:
                _renderManager.CenterCameraAroundPlayer();
                _renderManager.RenderGameObjects();
                break;

            case Scene.MainMenu:
            case Scene.Settings:
            case Scene.GameOver:
            case Scene.Win:
                _renderManager.RenderTextForScene(_uiManager.GetCurrentSceneTextElements(), _uiManager.Font);
                break;
        }
    }
}