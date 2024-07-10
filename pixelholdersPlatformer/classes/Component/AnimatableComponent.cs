using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.classes.managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.Component
{
    internal class AnimatableComponent : IComponent
    {

        private GameObject _owner;
        public string SpriteFolder;
        public AnimationType CurrentAnimationType;
        public IntPtr CurrentAnimationSprite;
        public bool isFlipped;
        public AnimatableComponent(GameObject owner, string spriteFolder)
        {
            _owner = owner;
            SpriteFolder = spriteFolder;
            CurrentAnimationType = AnimationType.Idle;
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
            PhysicsComponent tmp = ((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First());

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
                    SetAnimationType(AnimationType.Idle, isFlipped);
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
