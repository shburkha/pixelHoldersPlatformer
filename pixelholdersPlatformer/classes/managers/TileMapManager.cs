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

        foreach (var obj in _tilesets)
        {
            Console.WriteLine($"key: {obj.Key}, value: {obj.Value.Name}");
        }
    }
}