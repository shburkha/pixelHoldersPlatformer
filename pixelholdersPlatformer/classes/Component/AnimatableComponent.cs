using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.classes.managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace pixelholdersPlatformer.classes.Component
{
    internal class AnimatableComponent : IComponent
    {

        private GameObject _owner;
        public string SpriteFolder;
        public AnimationType CurrentAnimationType;
        public IntPtr CurrentAnimationSprite;
        public bool isFlipped;

        public SDL_Rect SpriteBoundingBox;

        private Stopwatch _attackTimer;
        private int _attackCooldown = 150;
        public AnimatableComponent(GameObject owner, string spriteFolder)
        {
            _owner = owner;
            SpriteFolder = spriteFolder;
            CurrentAnimationType = AnimationType.Idle;
            _attackTimer = new Stopwatch();
        }


        public void SetAttackCooldown(int cooldown)
        {
            _attackCooldown = cooldown;
        }

        public void SetAnimationType(AnimationType animationType, bool flipped = false) 
        {
            CurrentAnimationType = animationType;
            isFlipped = flipped;
        }

        public void SetAnimationSprite(IntPtr sprite) 
        {
            CurrentAnimationSprite = sprite;
        }

        public void Update()
        {

            
            PhysicsComponent tmp = (PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics);

            //when on the ground this is the default velocity
            if (tmp.Velocity.Y < 0.2 && tmp.Velocity.Y > 0)
            {
                if (tmp.Velocity.X < 0)
                {
                    SetAnimationType(AnimationType.Run, true);
                }
                else if (tmp.Velocity.X > 0)
                {
                    SetAnimationType(AnimationType.Run, false);
                }
                else if (tmp.Velocity.X == 0)
                {

                    if (CurrentAnimationType == AnimationType.Attack)
                    {
                        if (!_attackTimer.IsRunning)
                        {
                            _attackTimer.Start();
                        }
                        if (_attackTimer.ElapsedMilliseconds > _attackCooldown)
                        {
                            _attackTimer.Reset();
                            SetAnimationType(AnimationType.Idle, isFlipped);
                        }

                    }
                    else
                    {
                        SetAnimationType(AnimationType.Idle, isFlipped);
                    }
                }
            }
            else
            {
                if (tmp.Velocity.Y < 0)
                {
                    if (tmp.Velocity.X < 0)
                    {
                        SetAnimationType(AnimationType.Jump, true);
                    }
                    else if (tmp.Velocity.X > 0)
                    {
                        SetAnimationType(AnimationType.Jump, false);
                    }
                    else
                    {
                        SetAnimationType(AnimationType.Jump, isFlipped);
                    }
                    
                }
                else if (tmp.Velocity.Y > 0)
                {
                    if (tmp.Velocity.X < 0)
                    {
                        SetAnimationType(AnimationType.Fall, true);
                    }
                
                    else if (tmp.Velocity.X > 0)
                    {
                        SetAnimationType(AnimationType.Fall, false);
                    }
                    else
                    {
                        SetAnimationType(AnimationType.Fall, isFlipped);
                    }
                    
                }
            
            }
            
        }
    }
}
