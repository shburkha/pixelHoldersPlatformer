using pixelholdersPlatformer.classes.gameObjects;
using SDL2;
using System.Drawing;
using TiledCSPlus;
using static SDL2.SDL;

namespace pixelholdersPlatformer.classes.managers;

public class TileMapManager
{
    private TiledMap _map;
    private Dictionary<int, TiledTileset> _tilesets;
    int currentLevel = 1;

    public delegate void LevelAdvancedEventHandler();

    public event LevelAdvancedEventHandler OnLevelAdvanced;

    private int[] _collidableTiles =
    [
        21, 22, 23, 25, 40, 42, 44, 59, 60, 61, 63, 84, 85, 87, 88, 90,
        91, 93, 94, 97, 98, 99, 101, 103, 104, 106, 107, 109, 110, 112, 113, 257, 258, 259, 260,
        264, 265, 266, 267
    ];

    private int[] _winTiles = [256, 263, 270, 277];

    private static TileMapManager _instance;

    public static TileMapManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TileMapManager();
            return _instance;
        }
    }

    public TileMapManager()
    {
        _map = new TiledMap("assets/level1.tmx"); // tilesize is 32x32
        _tilesets = _map.GetTiledTilesets("assets/");
        /*
        foreach (var tileset in _tilesets)
        {
            // Console.WriteLine($"key: {tileset.Key}, value: {tileset.Value.Name}");
        }

        foreach (var layer in _map.Layers)
        {
            Console.WriteLine($"layer name: {layer.Name}");
            foreach (var entry in layer.Data)
            {
                if (entry != 0)
                    Console.WriteLine($"entry: {entry}");
            }
        }*/
    }

    public List<GameObject> GetEnvironmentCollidables()
    {
        List<GameObject> boxes = new List<GameObject>();

        //foreach (var layer in _map.Layers)
        //{
        //    int index = 0;
        //    foreach (var entry in layer.Data)
        //    {
        //        if (_collidableTiles.Contains(entry))
        //        {
        //            boxes.Add( new GameObject( index%layer.Width, index/layer.Width, 1, 1 ));
        //        }

        //        index++;
        //    }
        //}

        List<Tile> rects = ConsolidateCollisionBoxes(GenerateTerrainCollisionBoxes());

        foreach (var rect in rects)
        {
            boxes.Add(new GameObject(rect.x, rect.y, rect.w, rect.h));
        }

        // Console.WriteLine("Boxes: " + boxes.Count);
        return boxes;
    }

    private List<Tile> GenerateTerrainCollisionBoxes()
    {
        List<Tile> boxes = new List<Tile>();

        foreach (var layer in _map.Layers)
        {
            int index = 0;
            foreach (var entry in layer.Data)
            {
                if (_collidableTiles.Contains(entry))
                {
                    boxes.Add(new Tile { x = index % layer.Width, y = index / layer.Width, w = 1, h = 1 });
                }

                index++;
            }
        }

        return boxes;
    }

    private List<Tile> ConsolidateCollisionBoxes(List<Tile> boxes)
    {
        List<Tile> newBoxes = [.. boxes];
        do
        {
            boxes.Clear();
            boxes.AddRange(newBoxes);
            newBoxes.Clear();

            for (int i = 1; i < boxes.Count(); i++)
            {
                if (boxes[i - 1].x + boxes[i - 1].w == boxes[i].x && boxes[i - 1].y == boxes[i].y &&
                    boxes[i - 1].h == boxes[i].h)
                {
                    newBoxes.Add(new Tile
                    {
                        x = boxes[i - 1].x, y = boxes[i - 1].y, w = boxes[i - 1].w + boxes[i].w, h = boxes[i - 1].h
                    });
                    i++;
                }
                else
                {
                    newBoxes.Add(boxes[i - 1]);
                }

                if (i == boxes.Count() - 1)
                {
                    newBoxes.Add(boxes[i]);
                }
            }
        } while (newBoxes.Count() < boxes.Count());

        List<Tile> finalBoxes = new List<Tile>();

        for (int i = 0; i < newBoxes.Count; i++)
        {
            Tile current = newBoxes[i];

            for (int j = i + 1; j < newBoxes.Count; j++)
            {
                Tile next = newBoxes[j];

                if (current.x == next.x && current.w == next.w && current.y + current.h == next.y)
                {
                    current.h += next.h;
                    newBoxes.RemoveAt(j);
                    j--;
                }
            }

            finalBoxes.Add(current);
        }

        return finalBoxes;
    }

    public List<SpecialTile> GetSpecialTiles()
    {
        List<Tile> boxes = new List<Tile>();
        List<SpecialTile> list = new List<SpecialTile>();

        foreach (var layer in _map.Layers)
        {
            int index = 0;
            foreach (var entry in layer.Data)
            {
                if (_winTiles.Contains(entry))
                {
                    boxes.Add(new Tile { x = index % layer.Width, y = index / layer.Width, w = 1, h = 1 });
                }

                index++;
            }
        }

        boxes = ConsolidateCollisionBoxes(boxes);

        foreach (var box in boxes)
        {
            list.Add(new SpecialTile(box.x, box.y, box.w, box.h, SpecialTileType.Goal));
        }

        list.Add(new SpecialTile(0, _map.Height, _map.Width, 0, SpecialTileType.Kill));

        return list;
    }

    public void AdvanceLevel()
    {
        string path = "assets/level1.tmx";

        switch (currentLevel)
        {
            case 1:
                path = "assets/level2.tmx";
                break;
            case 2:
                path = "assets/level3.tmx";
                break;
            default:
                Console.WriteLine("Invalid level number");
                return;
        }

        _map = new TiledMap(path);
        _tilesets = _map.GetTiledTilesets("assets/");

        OnLevelAdvanced?.Invoke();
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

public struct Tile
{
    public float x;
    public float y;
    public float w;
    public float h;
}