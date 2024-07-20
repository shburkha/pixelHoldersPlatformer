using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using SDL2;
using TiledCSPlus;
using System.Numerics;
using static SDL2.SDL;
using SharpDX.Multimedia;

namespace pixelholdersPlatformer.classes.managers;

public class RenderManager
{
    //let's say that 10px is 1m
    //so the default screen size is 80m by 60m
    private int _defaultScreenWidth = 1280;
    private int _defaultScreenHeight = 720;

    private int _scaleX;
    private int _scaleY;

    private int _offsetX;
    private int _offsetY;

    private SDL_Rect _camera_rect;

    private nint _window;
    public nint _renderer;

    private GameObject _camera;
    private GameObject _map;


    private IntPtr _mapTexture;

    private MapData _mapData;
    private List<IntPtr> _tileSetTextures;


    private List<GameObject> _gameObjects;

    private int _zoomLevel;
    private bool _alwaysRender;



    //sprite pixel values for rendering them correctly
    private const int _humanKingSpriteWidth = 78;
    private const int _humanKingSpriteHeight = 58;
    private const int _humanKingTopPadding = 10;
    private const int _humanKingLeftPadding = 15;

    private const int _pigKingSpriteWidth = 38;
    private const int _pigKingSpriteHeight = 28;

    private const int _pigSpriteWidth = 34;
    private const int _pigSpriteHeight = 28;




    public RenderManager()
    {
        _window = SDL_CreateWindow("Platformer",
            SDL_WINDOWPOS_CENTERED,
            SDL_WINDOWPOS_CENTERED,
            _defaultScreenWidth,
            _defaultScreenHeight,
            SDL_WindowFlags.SDL_WINDOW_SHOWN);

        _renderer = SDL_CreateRenderer(_window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

        _camera = new GameObject(5, 5, 16, 9);
        _map = new GameObject(0, 0, 200, 50);

        _alwaysRender = true;

        _zoomLevel = 1;
        _scaleX = (int)(_defaultScreenWidth / _camera.Width) / _zoomLevel;
        _scaleY = (int)(_defaultScreenWidth / _camera.Width) / _zoomLevel;
        _offsetX = (int)((_defaultScreenWidth / 2) - (_camera.Width / 2) * _scaleX);
        _offsetY = (int)((_defaultScreenHeight / 2) - (_camera.Height / 2) * _scaleY);

        _camera_rect = new SDL_Rect
        {
            x = _offsetX,
            y = _offsetY,
            w = ((int)(_camera.Width * _scaleX)),
            h = ((int)(_camera.Height * _scaleY))
        };

        _tileSetTextures = new List<IntPtr>();
    }

    public void SetGameObjects(List<GameObject> gameObjects)
    {
        this._gameObjects = gameObjects;
        gameObjects.Add(_map);
        foreach (GameObject gameObject in gameObjects)
        {
            SetGameObjectBoundingBox(gameObject);
        }

    }

    private void SetGameObjectBoundingBox(GameObject gameObject)
    {
        ((RenderingComponent)gameObject.GetComponent(gameObjects.Component.Rendering))
            .BoundingBox =
            new SDL_Rect
            {
                x = ((int)((gameObject.CoordX - _camera.CoordX) * _scaleX)) + _offsetX,
                y = ((int)((gameObject.CoordY - _camera.CoordY) * _scaleY)) + _offsetY,
                w = ((int)(gameObject.Width * _scaleX)),
                h = ((int)(gameObject.Height * _scaleY))
            };

        if (gameObject.GetComponent(gameObjects.Component.Animatable) != null)
        {

            AnimatableComponent currentComponent = (AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable);
            string spriteFolder = currentComponent.SpriteFolder;


            int currentSpriteWidthInPixels = 0;
            int currentSpriteHeightInPixels = 0;
            int leftPadding = 0;
            int topPadding = 0;
            switch (spriteFolder)
            {
                case "01-King Human":
                    currentSpriteWidthInPixels = _humanKingSpriteWidth;
                    currentSpriteHeightInPixels= _humanKingSpriteHeight;
                    leftPadding = _humanKingLeftPadding;
                    topPadding = _humanKingTopPadding;

                    break;

                case "02-King Pig":
                    currentSpriteWidthInPixels = _pigKingSpriteWidth;
                    currentSpriteHeightInPixels = _pigKingSpriteHeight;
                    break;

                case "03-Pig":
                    currentSpriteWidthInPixels = _humanKingSpriteWidth;
                    currentSpriteHeightInPixels = _humanKingSpriteHeight;
                    break;


            }
            //we need a separate box for animation sprites, because they are bigger than the boundingbox of the player
            //we need to position the sprites so the center point of it aligns with the center point of the collision box
            //this way we can use the original sprites with the attack animation too
            //here we also set the sprites' boundingbox

            if (!((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable)).isFlipped)
            {
                ((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable))
                .SpriteBoundingBox =
                new SDL_Rect
                {
                    x = (int)((int)(((int)((gameObject.CoordX - _camera.CoordX) * _scaleX)) + _offsetX) - leftPadding / _zoomLevel),
                    y = (int)((int)(((int)((gameObject.CoordY - _camera.CoordY) * _scaleY)) + _offsetY) - topPadding / _zoomLevel),
                    w = (int)(currentSpriteWidthInPixels / _zoomLevel * 2),
                    h = (int)(currentSpriteHeightInPixels / _zoomLevel * 2)

                };

            }
            else
            {
                ((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable))
                .SpriteBoundingBox =
                new SDL_Rect
                {
                    x = (int)((int)(((int)((gameObject.CoordX - _camera.CoordX) * _scaleX)) + _offsetX) - (currentSpriteWidthInPixels - leftPadding) / _zoomLevel),
                    y = (int)((int)(((int)((gameObject.CoordY - _camera.CoordY) * _scaleY)) + _offsetY) - topPadding / _zoomLevel),
                    w = (int)(currentSpriteWidthInPixels / _zoomLevel * 2),
                    h = (int)(currentSpriteHeightInPixels / _zoomLevel * 2)

                };

            }


        }

    }

    public void WipeScreen()
    {
        SDL_SetRenderDrawColor(_renderer, 0, 0, 0, 0);
        SDL_RenderClear(_renderer);
    }

    private void DrawGameObject(GameObject gameObject)
    {
        SDL_SetRenderDrawColor(_renderer, 0, 0, 255, 255);
        SDL_RenderDrawRect(_renderer, ref _camera_rect);

        //first, we check if the gameObject is inside the camera's view
        if (_alwaysRender || IsInsideCameraView(gameObject))
        {

            SDL_SetRenderDrawColor(_renderer, 255, 255, 255, 255);
            SDL_RenderDrawRect(_renderer,
                ref ((RenderingComponent)gameObject.GetComponent(gameObjects.Component.Rendering)).BoundingBox);

            if (gameObject.GetComponent(gameObjects.Component.Animatable) != null)
            {
                SDL_SetRenderDrawColor(_renderer, 255, 0, 255, 255);
                SDL_RenderDrawRect(_renderer,
                    ref ((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable)).SpriteBoundingBox);
            }
            
        }
    }

    private void DrawSprite(GameObject gameObject) 
    {

        //SDL_SetTextureBlendMode(((AnimatableComponent)gameObject.Components.Where(t => t.GetType().Name == "AnimatableComponent").First()).CurrentAnimationSprite, SDL_BlendMode.SDL_BLENDMODE_BLEND);
        if (!((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable)).isFlipped)
        {
            SDL_RenderCopy(_renderer, ((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable)).CurrentAnimationSprite, (nint)null,
            ref ((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable)).SpriteBoundingBox);
        }
        else
        {
            SDL_RenderCopyEx(_renderer, ((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable)).CurrentAnimationSprite, 
                (nint)null, 
                ref ((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable)).SpriteBoundingBox,
                180,
                (nint)null, 
                SDL_RendererFlip.SDL_FLIP_VERTICAL);
        }
    }


    public void RenderGameObjects()
    {

        //SDL_RenderCopy(_renderer, _mapTexture, (nint)null, ref ((RenderingComponent)_map.Components.Where(t => t.GetType().Name == "RenderingComponent")
        //    .First()).BoundingBox);
        RenderMapFromTilemap();


        foreach (GameObject gameObject in _gameObjects)
        {
            if (gameObject.GetComponent(gameObjects.Component.Rendering) != null)
            {
                //this shouldn't be called every time...
                //TODO: make a boolean that checks for screen size changes
                SetGameObjectBoundingBox(gameObject);
            }
            if (gameObject.GetComponent(gameObjects.Component.Animatable) != null)
            {
                DrawSprite(gameObject);
                DrawGameObject(gameObject);

            }
            else
            {
                DrawGameObject(gameObject);
            }
            
        }
        SDL_RenderPresent(_renderer);
    }

    public void SwitchRenderMode()
    {
        _alwaysRender = !_alwaysRender;
    }

    public void MoveCamera(float distanceX, float distanceY)
    {
        _camera.CoordX += distanceX;
        _camera.CoordY += distanceY;
        if (_camera.CoordX + _camera.Width > _map.Width)
        {
            _camera.CoordX = _map.Width - _camera.Width;
        }
        if (_camera.CoordX < 0)
        {
            _camera.CoordX = 0;
        }
        if (_camera.CoordY + _camera.Height > _map.Height)
        {
            _camera.CoordY = _map.Height - _camera.Height;
        }
        if (_camera.CoordY < 0)
        {
            _camera.CoordY = 0;
        }
        foreach (GameObject gameObject in _gameObjects)
        {
            SetGameObjectBoundingBox(gameObject);
        }
    }

    public void Zoom(int amount)
    {
        _zoomLevel += amount;
        _zoomLevel = Math.Clamp(_zoomLevel, 1, 10); //does the same as before, just looks nicer and saves lines
        _scaleX = (int)(_defaultScreenWidth / _camera.Width) / _zoomLevel;
        _scaleY = (int)(_defaultScreenWidth / _camera.Width) / _zoomLevel;
        _offsetX = (int)((_defaultScreenWidth / 2) - (_camera.Width / 2) * _scaleX);
        _offsetY = (int)((_defaultScreenHeight / 2) - (_camera.Height / 2) * _scaleY);

        _camera_rect = new SDL_Rect
        {
            x = _offsetX,
            y = _offsetY,
            w = ((int)(_camera.Width * _scaleX)),
            h = ((int)(_camera.Height * _scaleY))
        };

        foreach (GameObject gameObject in _gameObjects)
        {
            SetGameObjectBoundingBox(gameObject);
        }
    }

    private bool IsInsideCameraView(GameObject gameObject)
    {
        SDL_Rect gameObject_rect = new SDL_Rect
        {
            x = ((int)((gameObject.CoordX - _camera.CoordX) * _scaleX)) + _offsetX,
            y = ((int)((gameObject.CoordY - _camera.CoordY) * _scaleY)) + _offsetY,
            w = ((int)(gameObject.Width * _scaleX)),
            h = ((int)(gameObject.Height * _scaleY))
        };

        if (SDL_HasIntersection(ref gameObject_rect, ref _camera_rect) == SDL_bool.SDL_TRUE)
            return true;

        return false;
    }

    public void SetZoomLevel(int level)
    {
        _zoomLevel = level;
        Zoom(0);
    }

    public void CenterCameraAroundPlayer()
    {
        var player = _gameObjects.Find(t => t.GetType().Name == "Player");

        if (player == null) { return; }

        Vector2 diff = new Vector2 { X = 0, Y = 0 };

        diff.X = (player.CoordX + player.Width / 2 - _camera.Width / 2) - _camera.CoordX;
        diff.Y = (player.CoordY + player.Height / 2 - _camera.Height / 2) - _camera.CoordY;

        MoveCamera(diff.X, diff.Y);
    }

    private void RenderMapFromTilemap()
    {
        int[] tilesetColumns = [_mapData.Tilesets[1].Columns, _mapData.Tilesets[248].Columns];
        int mapWidth = _mapData.Map.Width * _mapData.Tilesets[1].TileWidth;
        int mapHeight = _mapData.Map.Height * _mapData.Tilesets[1].TileHeight;
        IntPtr renderTarget = SDL_CreateTexture(_renderer,
            SDL_PIXELFORMAT_RGBA8888,
            (int)SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET,
            mapWidth, 
            mapHeight
        );
        SDL_SetRenderTarget(_renderer, renderTarget);

        foreach (var layer in _mapData.Map.Layers)
        {
            int key = layer.Id;
            if (key == 2)
            {
                key = 248;
            }

            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    int tileIndex = layer.Data[y * layer.Width + x];
                    if (tileIndex == 0) { continue; } //skip empty tiles

                    tileIndex -= (layer.Id - 1) * 247;
                    tileIndex--; // Tiled uses 1-based index, we need 0-based

                    int tilesetX = (tileIndex % tilesetColumns[layer.Id - 1]) * _mapData.Tilesets[key].TileWidth;
                    int tilesetY = (tileIndex / tilesetColumns[layer.Id - 1]) * _mapData.Tilesets[key].TileHeight;

                    SDL_Rect srcRect = new SDL_Rect
                    {
                        x = tilesetX,
                        y = tilesetY,
                        w = _mapData.Tilesets[key].TileWidth,
                        h = _mapData.Tilesets[key].TileHeight
                    };

                    SDL_Rect destRect = new SDL_Rect
                    {
                        x = x * _mapData.Tilesets[key].TileWidth,
                        y = y * _mapData.Tilesets[key].TileHeight,
                        w = _mapData.Tilesets[key].TileWidth,
                        h = _mapData.Tilesets[key].TileHeight
                    };

                    SDL_RenderCopy(_renderer, _tileSetTextures[layer.Id - 1], ref srcRect, ref destRect);
                }
            }
        }

        SDL_SetRenderTarget(_renderer, IntPtr.Zero);

        SDL_Rect renderDestRect = ((RenderingComponent)_map.GetComponent(gameObjects.Component.Rendering)).BoundingBox;

        SDL_RenderCopy(_renderer, renderTarget, IntPtr.Zero, ref renderDestRect);

        SDL_DestroyTexture(renderTarget);
    }

    public void SetMapData(MapData mapData)
    {
        _mapData = mapData;

        _map.Width = _mapData.Map.Width;
        _map.Height = _mapData.Map.Height;
        SetGameObjectBoundingBox(_map);

        foreach (var layer in _mapData.Map.Layers)
        {
            int key = layer.Id;
            if (key == 2)
            {
                key = 248;
            }

            _tileSetTextures.Add(SDL_image.IMG_LoadTexture(_renderer, "assets/TileSets/" + _mapData.Tilesets[key].Image.Source));
        }
    }
}