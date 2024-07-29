using pixelholdersPlatformer.classes.behaviours;
using pixelholdersPlatformer.classes.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.gameObjects
{
    public class Enemy : GameObject
    {
        private string _enemyType = "03-Pig";
        
        public bool IsHurt = false;
        public Enemy(float coordX, float coordY, float width, float height) : base(coordX, coordY, width, height)
        {
            this.Components.Add(new MovableComponent(this));
            this.Components.Add(new PhysicsComponent(this));
            this.Components.Add(new CollisionComponent(this));
            this.Components.Add(new AnimatableComponent(this, _enemyType));
            ((PhysicsComponent)GetComponent(Component.Physics)).HasGravity = true;
            ((PhysicsComponent)GetComponent(Component.Physics)).CanMove = true;
        }

        public void SetEnemyType(string enemyType)
        { 
            _enemyType = enemyType;

        }

    }

}
