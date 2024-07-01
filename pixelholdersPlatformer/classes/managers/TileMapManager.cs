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

    /*public void RenderTileMap()
    {
        var imgPath = "assets/map.png";

        IntPtr img = SDL_image.IMG_Load(imgPath);
        IntPtr texture = SDL_CreateTextureFromSurface(_renderer, img);

        SDL.SDL_Rect srcRect = new SDL_Rect
        {
            x = 0,
            y = 0,
            w = 100,
            h = 100
        };

        SDL_Rect dstRect = new SDL_Rect
        {
            x = 0,
            y = 0,
            w = 100,
            h = 100
        };

        SDL_RenderCopy(_renderer, texture, ref srcRect, ref dstRect);

        SDL_DestroyTexture(texture);
        SDL_FreeSurface(img);

        foreach (var layer in map.Layers)
        {
            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    var index = (y * layer.Width) + x;
                    var gid = layer.Data[index];
                    var tileX = (x * map.TileWidth);
                    var tileY = (y * map.TileHeight);

                    if (gid == 0)
                    {
                        continue;
                    }

                    var mapTileSet = map.GetTiledMapTileset(gid);
                    var tileset = tilesets[mapTileSet.FirstGid];
                    var rect = map.GetSourceRect(mapTileSet, tileset, gid);

                    RenderTile(rect, tileX, tileY, tileset);
                }
            }
        }
    }

    private void RenderTile(TiledSourceRect rect, int x, int y, TiledTileset tileset)
    {
        if (File.Exists(tileset.Image.Source))
        {
            Console.WriteLine($"File exists {tileset.Image.Source}");
        } else if (!File.Exists(tileset.Image.Source))
        {
            Console.WriteLine($"File doesn't exists {tileset.Image.Source}");
        }
        Console.WriteLine($"x: {x}, y: {y}, w: {rect.Width}, h: {rect.Height}");
        IntPtr image = SDL_image.IMG_Load(tileset.Image.Source);
        IntPtr texture = SDL_CreateTextureFromSurface(_renderer, image);

        SDL_Rect srcRect = new SDL_Rect
        {
            x = x,
            y = y,
            w = rect.Width,
            h = rect.Height
        };
        SDL_Rect dstRect = new SDL_Rect()
        {
            x = x,
            y = y,
            w = rect.Width,
            h = rect.Height
        };

        SDL_RenderCopy(_renderer, texture, ref srcRect, ref dstRect);
    }*/
}