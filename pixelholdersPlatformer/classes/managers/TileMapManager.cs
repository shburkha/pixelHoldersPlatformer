using SDL2;
using TiledCSPlus;
using static SDL2.SDL;

namespace pixelholdersPlatformer.classes.managers;

public class TileMapManager
{
    private TiledMap _map;
    private Dictionary<int, TiledTileset> _tilesets;

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
}