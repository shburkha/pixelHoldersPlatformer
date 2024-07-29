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

        public int EnemyHealth;
        private const double _invincibleTime = 1.0d; //in seconds
        private double _timeSinceLastDamage = 0.0d; //in seconds

        public bool IsHurt = false;
        public Enemy(float coordX, float coordY, float width, float height) : base(coordX, coordY, width, height)
        {
            EnemyHealth = 3;
            this.Components.Add(new MovableComponent(this));
            this.Components.Add(new PhysicsComponent(this));
            this.Components.Add(new CollisionComponent(this));
            this.Components.Add(new AnimatableComponent(this, _enemyType));
            ((PhysicsComponent)GetComponent(Component.Physics)).HasGravity = true;
            ((PhysicsComponent)GetComponent(Component.Physics)).CanMove = true;
        }


        public void HurtEnemy()
        {
            if (_timeSinceLastDamage <= _invincibleTime) { return; }
            EnemyHealth -= 1;
            _timeSinceLastDamage = 0;
        }

        public void SetEnemyType(string enemyType)
        {
            if (enemyType == "02-King Pig")
            {
                EnemyHealth = 6;
            }
            Components.Remove(this.GetComponent(Component.Animatable) as AnimatableComponent);
            Components.Add(new AnimatableComponent(this, enemyType));
            _enemyType = enemyType;

        }
        public override void Update()
        {
            _timeSinceLastDamage += ((PhysicsComponent)GetComponent(Component.Physics)).DeltaT;
            base.Update();
        }

    }

}
