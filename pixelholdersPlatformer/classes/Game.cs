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

    private double _frameInterval;
    private RenderManager _renderManager;
    public Game()
    {
        _renderManager = new RenderManager();
        gameObjects = new List<GameObject>();

        gameObjects.Add(new GameObject(51, 51, 2, 2));
        gameObjects.Add(new GameObject(45, 50, 2, 2));
        gameObjects.Add(new GameObject(0, 0, 100, 100));
        SDL_DisplayMode _displayMode;
        SDL_GetCurrentDisplayMode(0, out _displayMode);
        int _refreshRate = _displayMode.refresh_rate;
        double targetFPS = _refreshRate * 1.0d;
        _frameInterval = 1000d / targetFPS;
        stopwatch.Start();


    }


    public void StartGame()
    {
        while (true)
        {
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
        int numKeys;
        IntPtr keyStatePtr = SDL.SDL_GetKeyboardState(out numKeys);
        byte[] keyState = new byte[numKeys];
        System.Runtime.InteropServices.Marshal.Copy(keyStatePtr, keyState, 0, numKeys);
        int w = keyState[(int)SDL.SDL_Scancode.SDL_SCANCODE_W];
        int a = keyState[(int)SDL.SDL_Scancode.SDL_SCANCODE_A];
        int s = keyState[(int)SDL.SDL_Scancode.SDL_SCANCODE_S];
        int d = keyState[(int)SDL.SDL_Scancode.SDL_SCANCODE_D];
        int plus = keyState[(int)SDL.SDL_Scancode.SDL_SCANCODE_KP_PLUS];
        int minus = keyState[(int)SDL.SDL_Scancode.SDL_SCANCODE_KP_MINUS];
        int r = keyState[(int)SDL.SDL_Scancode.SDL_SCANCODE_R];
        if (w != 0)
        {
            _renderManager.MoveCamera(0, -1);
        }
        if (s != 0)
        {
            _renderManager.MoveCamera(0, 1);
        }
        if (a != 0)
        {
            _renderManager.MoveCamera(-1, 0);
        }

        if (d != 0)
        {
            _renderManager.MoveCamera(1, 0);
        }
        if (r != 0)
        {
            _renderManager.SwitchRenderMode();
        }
        if(plus != 0) 
        {
            _renderManager.Zoom(-5);
        }
        if(minus != 0) 
        {
            _renderManager.Zoom(5);
        }

        SDL_Event e;
        while (SDL_PollEvent(out e) != 0)
        {

        } 
    }
        

    private void update()
    {

    }

    private void render()
    {
        _renderManager.wipeScreen();
        foreach (GameObject gameObject in gameObjects) 
        {            
            _renderManager.RenderGameObject(gameObject);

        }
    }
}