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
            if (overlapY != 0f && ((PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics)).HasGravity)
            {
                ((MovableComponent)_owner.GetComponent(gameObjects.Component.Movable)).MoveGameObject(0, -overlapY);
                if (((PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics)).Velocity.Y < 0)
                {
                    ((PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics)).Velocity.Y = 0.001f;
                }
                else
                {
                    ((PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics)).Velocity.Y = 0;
                }
            }
            if (overlapX != 0f && ((PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics)).CanMove)
            {
                ((MovableComponent)_owner.GetComponent(gameObjects.Component.Movable)).MoveGameObject(-overlapX, 0);
                ((PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics)).Velocity.X = 0;
            }
        }
        public void Update()
        {
            
        }
    }
}
