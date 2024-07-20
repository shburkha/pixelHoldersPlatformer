using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using SDL2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace pixelholdersPlatformer.classes.managers
{
    public class AnimationManager
    {
        //the human king's picture height is 78px and with is 58px
        //the pig king's picture height is 38px is 28px
        //the pigs' picture height is 34px is 28px

        private const int _humanKingSpriteWidth = 78;
        private const int _humanKingSpriteHeight = 58;

        private const int _pigKingSpriteWidth = 38;
        private const int _pigKingSpriteHeight = 28;

        private const int _pigSpriteWidth = 34;
        private const int _pigSpriteHeight = 28;
        

        private Dictionary<string, IntPtr[]> _humanKingSprites;
        private Dictionary<string, IntPtr[]> _pigKingSprites;
        private Dictionary<string, IntPtr[]> _pigSprites;
        
        private RenderManager _renderManager;

        private Stopwatch _spriteChangeStopWatch;


        private List<GameObject> _gameObjects;

        public void SetGameObjects(List<GameObject> gameObjects)
        {
            this._gameObjects = gameObjects;
        }

        public AnimationManager(RenderManager renderManager)
        {
            _renderManager = renderManager;
            _spriteChangeStopWatch = new Stopwatch();
            _spriteChangeStopWatch.Start();
            LoadAnimationSprites();
            

        }

        private void LoadAnimationSprites()
        {
            string spriteFoldersPath = Path.Combine(Directory.GetCurrentDirectory().Replace("bin\\Debug\\net8.0", ""), Path.Combine("assets", "sprites"));
            string[] spriteFolderNames = Directory.GetDirectories(spriteFoldersPath);
            for (int i = 0; i < spriteFolderNames.Length; i++)
            {
                LoadAnimationSprite(spriteFolderNames[i].Split("\\")[spriteFolderNames[i].Split("\\").Length-1]);
            }

        }
        
        private void LoadAnimationSprite(string folderName)
        {
            string spritesPath = Path.Combine(Directory.GetCurrentDirectory().Replace("bin\\Debug\\net8.0", ""), Path.Combine("assets", Path.Combine("sprites", folderName)));
            string[] spriteNames = Directory.GetFiles(spritesPath);
            switch (folderName)
            {
                case "01-King Human":
                    _humanKingSprites = new Dictionary<string, IntPtr[]>();
                    
                    foreach (string spriteName in spriteNames)
                    {
                        //we get the big sprite's width, so we can divide it accordingly
                        Image img = Image.FromFile(spriteName);


                       
                        int currentManKingSpriteCount = img.Width / _humanKingSpriteWidth;

                        IntPtr[] currentSprites = new IntPtr[currentManKingSpriteCount];

                        IntPtr imgSurface = SDL_image.IMG_Load(spriteName);
                        for (int i = 0; i < currentManKingSpriteCount; i++)
                        {
                            IntPtr surface = CreateSDLSurfaceFromImage(imgSurface, i, _humanKingSpriteWidth, _humanKingSpriteHeight);
                            currentSprites[i] = CreateTextureFromSurface(surface);
                        }
                        _humanKingSprites.Add(spriteName.Split("\\")[spriteName.Split("\\").Length-1].Split(" (")[0], currentSprites);

                    }

                    break;


                case "02-King Pig":

                    _pigKingSprites = new Dictionary<string, IntPtr[]>();
                    foreach (string spriteName in spriteNames)
                    {
                        //we get the big sprite's width so we can divide it accordingly
                        Image img = Image.FromFile(spriteName);

                        int currentPigSpriteCount = img.Width / _pigKingSpriteWidth;

                        IntPtr[] currentSprites = new IntPtr[currentPigSpriteCount];

                        IntPtr imgSurface = SDL_image.IMG_Load(spriteName);
                        for (int i = 0; i < currentPigSpriteCount; i++)
                        {
                            IntPtr surface = CreateSDLSurfaceFromImage(imgSurface, i, _pigKingSpriteWidth, _pigKingSpriteHeight);
                            currentSprites[i] = CreateTextureFromSurface(surface);
                        }
                        _pigKingSprites.Add(spriteName.Split("\\")[spriteName.Split("\\").Length - 1].Split(" (")[0], currentSprites);

                    }

                    break;


                case "03-Pig":
                    _pigSprites = new Dictionary<string, IntPtr[]>();
                    foreach (string spriteName in spriteNames)
                    {
                        //we get the big sprite's width so we can divide it accordingly
                        Image img = Image.FromFile(spriteName);

                        int currentPigSpriteCount = img.Width / _pigSpriteWidth;

                        IntPtr[] currentSprites = new IntPtr[currentPigSpriteCount];

                        IntPtr imgSurface = SDL_image.IMG_Load(spriteName);
                        for (int i = 0; i < currentPigSpriteCount; i++)
                        {
                            IntPtr surface = CreateSDLSurfaceFromImage(imgSurface, i, _pigSpriteWidth, _pigSpriteHeight);
                            currentSprites[i] = CreateTextureFromSurface(surface);
                        }
                        _pigSprites.Add(spriteName.Split("\\")[spriteName.Split("\\").Length - 1].Split(" (")[0], currentSprites);

                    }


                    break;
                
            }
        }

        private IntPtr CreateSDLSurfaceFromImage(IntPtr imgSurface, int spriteIndex, int currentWidth, int currentHeight)
        {
            //SDL_Surface originalSurface = Marshal.PtrToStructure<SDL_Surface>(imgSurface);
            SDL_Rect spriteRect = new SDL_Rect
            {
                x = spriteIndex * currentWidth,
                y = 0,
                w = currentWidth,
                h = currentHeight
            };

            IntPtr spriteSurface = SDL_CreateRGBSurfaceWithFormat(0, currentWidth, currentHeight, 32, SDL_PIXELFORMAT_RGBA4444);
            SDL_BlitSurface(imgSurface, ref spriteRect, spriteSurface, IntPtr.Zero);
            return spriteSurface;
        }

        private IntPtr CreateTextureFromSurface(IntPtr surface)
        {
            IntPtr texture = SDL_CreateTextureFromSurface(_renderManager._renderer, surface);
            SDL_FreeSurface(surface); // Free the surface after creating the texture
            return texture;
        }



        private double _timeElapsed = 51; //start with an update
        private double _animationCooldown = 50; //ms
        public void AnimateObjects()
        {
            if (_timeElapsed < _animationCooldown)
            {
                _timeElapsed = (double)_spriteChangeStopWatch.ElapsedMilliseconds;
                return;
            }
            else
            {
                _timeElapsed = 0;
                _spriteChangeStopWatch.Restart();
            }
            //here we change the sprite according to the AnimationComponent's AnimationType
            foreach (GameObject gameObject in _gameObjects)
            {
                if (gameObject.GetComponent(gameObjects.Component.Animatable) != null)
                {
                    //we need to check which spriteFolder it uses
                    //the we need to check the the animationType


                    AnimatableComponent currentComponent = (AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable);
                    string spriteFolder = currentComponent.SpriteFolder;
                    AnimationType currentAnimationType = currentComponent.CurrentAnimationType;
                    string animationTypeInString = currentAnimationType.ToString();
                    IntPtr currentAnimationSprite = currentComponent.CurrentAnimationSprite;

                    Dictionary<string, IntPtr[]> currentSprites = null;

                    switch (spriteFolder)
                    {
                        case "01-King Human":

                            currentSprites = _humanKingSprites;
                            break;

                        case "02-King Pig":
                            currentSprites = _pigKingSprites;
                            break;

                        case "03-Pig":

                            currentSprites = _pigSprites;
                            break;


                    }

                    for (int i = 0; i < currentSprites[animationTypeInString].Length; i++)
                    {
                        if (currentSprites[animationTypeInString][i] == currentAnimationSprite)
                        {
                            if (i == currentSprites[animationTypeInString].Length - 1)
                            {
                                ((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable)).CurrentAnimationSprite = currentSprites[animationTypeInString][0];
                                break;
                            }
                            else
                            {
                                ((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable)).CurrentAnimationSprite = currentSprites[animationTypeInString][i + 1];
                                break;
                            }
                        }
                        else
                        {
                            ((AnimatableComponent)gameObject.GetComponent(gameObjects.Component.Animatable)).CurrentAnimationSprite = currentSprites[animationTypeInString][0];
                            
                        }
                    }
                }
            }
        }
    }


    public enum AnimationType
    { 
        Attack, Dead, Fall, Hit, Idle, Jump, Run,  
    }
}
