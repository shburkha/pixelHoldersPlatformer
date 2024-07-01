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
    private CollisionManager _collisionManager;
    public Game()
    {
        _renderManager = new RenderManager();
        _inputManager = new InputManager();
        _collisionManager = new CollisionManager();

        gameObjects = new List<GameObject>();
        _quit = false;

        GameObject border = new GameObject(0, 0, 100, 100);
        GameObject platform = new GameObject(50, 50, 10, 2);
        platform.AddComponent(new MovableComponent(platform));
        platform.AddComponent(new PhysicsComponent(platform));
        platform.AddComponent(new CollisionComponent(platform));
        GameObject wall = new GameObject(50, 40, 1, 10);
        wall.AddComponent(new MovableComponent(wall));
        wall.AddComponent(new PhysicsComponent(wall));
        wall.AddComponent(new CollisionComponent(wall));
        GameObject ceiling = new GameObject(50, 55, 10, 2);
        ceiling.AddComponent(new MovableComponent(ceiling));
        ceiling.AddComponent(new PhysicsComponent(ceiling));
        ceiling.AddComponent(new CollisionComponent(ceiling));
        _player = new Player(52, 52, 1, 1);


        gameObjects.Add(border);
        gameObjects.Add(platform);
        gameObjects.Add(ceiling);
        //gameObjects.Add(wall);
        gameObjects.Add(_player);

        _renderManager.SetGameObjects(gameObjects);
        _collisionManager.SetGameObjects(gameObjects);

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
                    _player.MovePlayerX(-0.5f);
                    break;
                case InputTypes.PlayerRight:
                    _player.MovePlayerX(0.5f);

                    break;
                case InputTypes.PlayerJump:
                    if (((PhysicsComponent)_player.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).Velocity.Y == 0)
                    {
                        _player.MovePlayerY(-2);
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
                    _renderManager.MoveCamera(-0.5f, 0);
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

            }
        }
    }

    private void update()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.Update();
        }
        _collisionManager.HandleCollision();
    }

    private void render()
    {
        _renderManager.WipeScreen();
        _renderManager.RenderGameObjects();
        
    }
}