using pixelholdersPlatformer.classes.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace pixelholdersPlatformer.classes.gameObjects
{
    public class Cannon : GameObject
    {
        private const float _width = 0.6f;
        private const float _height = 0.5f;

        private double _timeSinceLastShot = 0.0d; //in seconds
        private const double _shootSpeed = 1.5d; //in seconds

        private float[] _cannonballOffset = [0.3f, 0.1f];

        private Direction _direction;
        private bool _isFlipped = false;

        public Cannon(float coordX, float coordY, Direction direction) : base(coordX, coordY, _width, _height)
        {
            AddComponent(new PhysicsComponent(this));
            AddComponent(new AnimatableComponent(this, "10-Cannon"));
            AddComponent(new CollisionComponent(this));

            _direction = direction;

            if (_direction == Direction.Right)
            {
                _cannonballOffset[0] = (-1 * _cannonballOffset[0]) + Width;
                _isFlipped = true;
            }

            ((AnimatableComponent)GetComponent(Component.Animatable)).SetAnimationType(managers.AnimationType.Idle, _isFlipped);
        }

        public void SetDeltaTime(double dt)
        {
            ((PhysicsComponent)GetComponent(Component.Physics)).DeltaT = dt;
            _timeSinceLastShot += dt;
        }

        public Cannonball Shoot()
        {
            _timeSinceLastShot = 0d;

            ((AnimatableComponent)GetComponent(Component.Animatable)).SetAnimationType(managers.AnimationType.Attack, _isFlipped);

            return new Cannonball(CoordX + _cannonballOffset[0], CoordY + _cannonballOffset[1], 0.3f, 0.3f, _direction);
        }

        public bool CanShoot()
        {
            return _timeSinceLastShot >= _shootSpeed;
        }
    }
}

public enum Direction
{
    Left, Up, Right, Down
}