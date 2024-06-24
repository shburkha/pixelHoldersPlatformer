using TiledCSPlus;

namespace pixelholdersPlatformer.classes;

public class Game
{
    Gamepad _gamepad;
    int _playerX;
    int _playerY;
    int _speed;

    private TiledMap _map;
    private TiledTileset _decoTileset;
    private TiledTileset _terrainTileset;
    private TiledLayer _myLayer;
    private TiledObject _myObject;

    public Game()
    {
        _gamepad = new Gamepad();
        _playerX = 0;
        _playerY = 0;
        _speed = 5;

        _map = new TiledMap("assets/test.tmx");
        _decoTileset = new TiledTileset("assets/Decorations.tsx");
        _terrainTileset = new TiledTileset("assets/Terrain.tsx");
        _myLayer = _map.Layers.First();
        Console.WriteLine($"Map: {_map}");
        Console.WriteLine($"Decoration: {_decoTileset}");
        Console.WriteLine($"Terrain: {_terrainTileset}");
        Console.WriteLine($"Layer: {_myLayer}");
        Console.WriteLine($"Object: {_myLayer}");
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
        if (_gamepad.Joystick != null)
        {
            _gamepad.ProcessInput();
        }
    }

    private void Update()
    {
    }

    private void Render()
    {
    }
}