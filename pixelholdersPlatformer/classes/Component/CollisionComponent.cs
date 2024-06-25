using pixelholdersPlatformer.classes.gameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.Component
{
    internal class CollisionComponent : IComponent
    {


        private GameObject _owner;

        public bool IsCollidable { get;  set; }

        public CollisionComponent(GameObject owner)
        {
            _owner = owner;
            IsCollidable = true;
        }


        public void Collide(float overlapX, float overlapY)
        {
            if (((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).IsFallable)
            {

                Vector2 velocity = ((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).Velocity;
                if (velocity.Y > 0)
                {
                    ((MovableComponent)_owner.Components.Where(t => t.GetType().Name == "MovableComponent").First()).MoveGameObject(0, -overlapY);
                    ((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).SetVelocityY(-9.8f * 0.016f);

                }
                else if (velocity.Y < 0)
                {
                    ((MovableComponent)_owner.Components.Where(t => t.GetType().Name == "MovableComponent").First()).MoveGameObject(0, overlapY);
                    ((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).SetVelocityY(0);
                }

            }
        }

        public void Update()
        {
            
        }
    }
}
