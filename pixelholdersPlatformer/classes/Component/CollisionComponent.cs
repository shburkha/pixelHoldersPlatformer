using pixelholdersPlatformer.classes.gameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public void Collide()
        {
            if (((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).IsFallable)
            {
                ((PhysicsComponent)_owner.Components.Where(t => t.GetType().Name == "PhysicsComponent").First()).SetVelocity(0,0);
            }
            

        }

        public void Update()
        {
            
        }
    }
}
