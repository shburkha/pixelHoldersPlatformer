using pixelholdersPlatformer.classes.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.gameObjects
{
    public class Cannonball : GameObject
    {
        public float Velocity = 0.1f;
        public Direction Direction;
        public Cannonball(float coordX, float coordY, float width, float height, Direction direction) : base(coordX, coordY, width, height)
        {
            AddComponent(new AnimatableComponent(this, "15-Cannonball"));
            AddComponent(new CollisionComponent(this));
            AddComponent(new MovableComponent(this));
            AddComponent(new PhysicsComponent(this));
            ((PhysicsComponent)GetComponent(Component.Physics)).CanMove = true;

            this.Direction = direction;
            if (direction == Direction.Left)
            {
                Velocity *= -1;
            }
        }

        public void SetDeltaTime(double dt)
        {
            ((PhysicsComponent)GetComponent(Component.Physics)).DeltaT = dt;
        }

        public override void Update()
        {
            ((PhysicsComponent)GetComponent(Component.Physics)).SetVelocityX(Velocity);
            base.Update();
        }
    }
}
