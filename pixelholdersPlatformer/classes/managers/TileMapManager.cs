using pixelholdersPlatformer.classes.gameObjects;
using SDL2;
using TiledCSPlus;
using static SDL2.SDL;

namespace pixelholdersPlatformer.classes.managers;

public class TileMapManager
{
    private TiledMap _map;
    private Dictionary<int, TiledTileset> _tilesets;
    int currentLevel = 0;

    private int[] _collidableTiles =
    [
        21, 22, 23, 25, 40, 42, 44, 59, 60, 61, 63, 84, 85, 87, 88, 90,
        91, 93, 94, 97, 98, 99, 101, 103, 104, 106, 107, 109, 110, 112, 113, 257, 258, 259, 260,
        264, 265, 266, 267
    ];

    private int[] _winTiles = [256, 263, 270, 277];

    public TileMapManager()
    {
        _map = new TiledMap("assets/level1.tmx"); // tilesize is 32x32
        _tilesets = _map.GetTiledTilesets("assets/");

        foreach (var tileset in _tilesets)
        {
            Console.WriteLine($"key: {tileset.Key}, value: {tileset.Value.Name}");
        }

        foreach (var layer in _map.Layers)
        {
            Console.WriteLine($"layer name: {layer.Name}");
            foreach (var entry in layer.Data)
            {
                if (entry != 0)
                    Console.WriteLine($"entry: {entry}");
            }
        }
    }

    public List<GameObject> GetEnvironmentCollidables()
    {
        List<GameObject> boxes = new List<GameObject>();

        foreach (var layer in _map.Layers)
        {
            int index = 0;
            foreach (var entry in layer.Data)
            {
                if (_collidableTiles.Contains(entry))
                {
                    boxes.Add(new GameObject(index % layer.Width, index / layer.Width, 1, 1));
                }

                index++;
            }
        }

        Console.WriteLine("Boxes: " + boxes.Count);
        return boxes;
    }

    public List<SpecialTile> GetSpecialTiles()
    {
        List<SpecialTile> list = new List<SpecialTile>();

        foreach (var layer in _map.Layers)
        {
            int index = 0;
            foreach (var entry in layer.Data)
            {
                if (_winTiles.Contains(entry))
                {
                    list.Add(new SpecialTile(index % layer.Width, index / layer.Width, 1, 1, SpecialTileType.Goal));
                }
                index++;
            }
        }

        return list;
    }

    public void AdvanceLevel()
    {
        string path = "assets/level1.tmx";

        switch (currentLevel)
        {
            case 1:
                path = "assets/level1.tmx";
                break;
            case 2:
                path = "assets/level2.tmx";
                break;
            case 3:
                path = "assets/level3.tmx";
                break;
            default:
                Console.WriteLine("Invalid level number");
                return;
        }
        _map = new TiledMap(path);
        _tilesets = _map.GetTiledTilesets("assets/");
    }

    public MapData GetMapData()
    {
        return new MapData { Map = _map, Tilesets = _tilesets, LevelIndex = 0 };
    }
}

public struct MapData
{
    public TiledMap Map;
    public Dictionary<int, TiledTileset> Tilesets;
    public int LevelIndex;
}