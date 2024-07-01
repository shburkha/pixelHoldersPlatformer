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
            if (overlapY != 0f && ((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).HasGravity)
            {
                ((MovableComponent)_owner.Components.Where(t => t.GetType().Name == "MovableComponent").First()).MoveGameObject(0, -overlapY);
                ((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).Velocity.Y = 0;
            }
            if (overlapX != 0f && ((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).CanMove)
            {
                ((MovableComponent)_owner.Components.Where(t => t.GetType().Name == "MovableComponent").First()).MoveGameObject(-overlapX, 0);
                ((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).Velocity.X = 0;
            }
        }
        public void Update()
        {
            
        }
    }
}
