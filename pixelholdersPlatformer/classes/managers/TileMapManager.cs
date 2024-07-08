using pixelholdersPlatformer.classes.gameObjects;
using SDL2;
using TiledCSPlus;
using static SDL2.SDL;

namespace pixelholdersPlatformer.classes.managers;

public class TileMapManager
{
    private TiledMap _map;
    private Dictionary<int, TiledTileset> _tilesets;

    private int[] _collidableTiles = [21, 22, 23, 25, 40, 42, 44, 59, 60, 61, 63, 84, 85, 87, 88, 90, 91, 93, 94, 97, 98, 99, 101, 103, 104, 106, 107, 109, 110, 112, 113, 
        264, 265, 266, 267];

    public TileMapManager()
    {
        _map = new TiledMap("assets/map.tmx"); // tilesize is 32x32
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
                    boxes.Add( new GameObject( 3.34f*(index%layer.Width), 2.5f*(index/layer.Width), 3.34f, 2.5f ));
                }

                index++;
            }
        }
        Console.WriteLine("Boxes: "+boxes.Count);
        return boxes;
    }

    public MapData GetMapData()
    {
        return new MapData { Map = _map, Tilesets = _tilesets, LevelIndex = 0};
    }
}

public struct MapData
{
    public TiledMap Map;
    public Dictionary<int, TiledTileset> Tilesets;
    public int LevelIndex;
}