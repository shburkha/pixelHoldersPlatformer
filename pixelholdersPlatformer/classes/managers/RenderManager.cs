﻿using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using SDL2;
using TiledCSPlus;
using static SDL2.SDL;

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
    private nint _renderer;

    private GameObject _camera;
    private GameObject _map;

    private List<GameObject> gameObjects;

    private int _zoomLevel;
    private bool _alwaysRender;

    public RenderManager()
    {
        _window = SDL_CreateWindow("Platformer",
            SDL_WINDOWPOS_CENTERED,
            SDL_WINDOWPOS_CENTERED,
            _defaultScreenWidth,
            _defaultScreenHeight,
            SDL_WindowFlags.SDL_WINDOW_SHOWN);

        _renderer = SDL_CreateRenderer(_window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

        _camera = new GameObject(50, 50, 20, 15);
        _map = new GameObject(0, 0, 100, 100);

        _alwaysRender = false;

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
    }

    public void SetGameObjects(List<GameObject> gameObjects)
    {
        this.gameObjects = gameObjects;
        foreach (GameObject gameObject in gameObjects)
        {
            SetGameObjectBoundingBox(gameObject);
        }
    }

    private void SetGameObjectBoundingBox(GameObject gameObject)
    {
        ((RenderingComponent)gameObject.Components.Where(t => t.GetType().Name == "RenderingComponent").First())
            .BoundingBox =
            new SDL_Rect
            {
                x = ((int)((gameObject.CoordX - _camera.CoordX) * _scaleX)) + _offsetX,
                y = ((int)((gameObject.CoordY - _camera.CoordY) * _scaleY)) + _offsetY,
                w = ((int)(gameObject.Width * _scaleX)),
                h = ((int)(gameObject.Height * _scaleY))
            };
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
            if (nameof(gameObject) == "map")
            {
                var imgPath = "assets/map.png";

                IntPtr img = SDL_image.IMG_Load(imgPath);
                IntPtr texture = SDL_CreateTextureFromSurface(_renderer, img);


                SDL_RenderCopy(_renderer, texture, (nint)null, ref ((RenderingComponent)gameObject.Components.Where(t => t.GetType().Name == "RenderingComponent")
                    .First()).BoundingBox);

                SDL_DestroyTexture(texture);
                SDL_FreeSurface(img);
            }
            else
            {
                SDL_SetRenderDrawColor(_renderer, 255, 255, 255, 255);
                SDL_RenderDrawRect(_renderer,
                    ref ((RenderingComponent)gameObject.Components.Where(t => t.GetType().Name == "RenderingComponent")
                        .First()).BoundingBox);
            }

        }
    }

    public void RenderGameObjects()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.Components.Find(t => t.GetType().Name == "RenderingComponent") != null)
            {
                SetGameObjectBoundingBox(gameObject);
            }


            DrawGameObject(gameObject);
        }

        //RenderTileMap();
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
            _camera.CoordY = _map.Width - _camera.Height;
        }

        if (_camera.CoordY < 0)
        {
            _camera.CoordY = 0;
        }

        foreach (GameObject gameObject in gameObjects)
        {
            SetGameObjectBoundingBox(gameObject);
        }
    }

    public void Zoom(int amount)
    {
        _zoomLevel += amount;
        if (_zoomLevel < 1) _zoomLevel = 1;
        else if (_zoomLevel > 10) _zoomLevel = 10;
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

        foreach (GameObject gameObject in gameObjects)
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

    private IntPtr LoadTexture(string path)
    {
        //The final optimized image
        var newTexture = IntPtr.Zero;

        //Load image at specified path
        var loadedSurface = SDL_image.IMG_Load(path);
        if (loadedSurface == IntPtr.Zero)
        {
            Console.WriteLine("Unable to load image {0}! SDL Error: {1}", path, SDL.SDL_GetError());
        }
        else
        {
            //Create texture from surface pixels
            newTexture = SDL.SDL_CreateTextureFromSurface(_renderer, loadedSurface);
            if (newTexture == IntPtr.Zero)
                Console.WriteLine("Unable to create texture from {0}! SDL Error: {1}", path, SDL.SDL_GetError());

            //Get rid of old loaded surface
            SDL.SDL_FreeSurface(loadedSurface);
        }

        return newTexture;
    }

    public void RenderTileMap()
    {
        var imgPath = "assets/map.png";

        IntPtr img = SDL_image.IMG_Load(imgPath);
        IntPtr texture = SDL_CreateTextureFromSurface(_renderer, img);

        SDL_Rect dstRect = new SDL_Rect
        {
            x = 0,
            y = 0,
            w = 3200,
            h = 1504
        };

        SDL_RenderCopy(_renderer, texture, (nint)null, ref dstRect);

        SDL_DestroyTexture(texture);
        SDL_FreeSurface(img);
    }
}