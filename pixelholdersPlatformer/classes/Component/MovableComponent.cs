using pixelholdersPlatformer.classes.gameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.Component
{
    public class MovableComponent : IComponent
    {
        private GameObject _owner;
        public MovableComponent(GameObject owner) 
        {
            _owner = owner; 
        }
        public void Update()
        {
            
        }

        public void MoveGameObject(float velocityX, float velocityY)
        {
            _owner.CoordX += velocityX;
            _owner.CoordY += velocityY;
        }
    }
}
