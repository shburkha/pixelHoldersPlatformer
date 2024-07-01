﻿using pixelholdersPlatformer.classes.gameObjects;
using System.Numerics;

namespace pixelholdersPlatformer.classes.Component;

public class PhysicsComponent : IComponent
{

    private GameObject _owner;

    private const double GRAVITY = 5.5d;
    private const double AIR_RESISTANCE = 1.5d;
    private const double FRICTION = 5.0d;

    public double DeltaT = 0.016d;

    private Vector2 _velocity;

    public PhysicsComponent(GameObject owner)
    {
        _owner = owner;
    }

    public void SetVelocity(float amountX, float amountY)
    {
        _velocity.X = amountX;
        _velocity.Y = amountY;
    }

    public void SetVelocityX(float amountX)
    {
        _velocity.X = amountX;
    }
    public void SetVelocityY(float amountY)
    {
        _velocity.Y = amountY;
    }

    bool isColliding = true;
    public void Update()
    {
        if (_owner.CoordY >= 100 - _owner.Height)
        {
            _owner.CoordY = 100 - _owner.Height;
            if (_velocity.Y > 0 ) 
            {
                _velocity.Y = 0;
            }
        }
        else
        {
            _velocity.Y += (float)(GRAVITY * DeltaT);
        }

        if (_owner.CoordX != Math.Clamp(_owner.CoordX, 0, 100 - _owner.Width))
        {
            _owner.CoordX = Math.Clamp(_owner.CoordX, 0, 100 - _owner.Width);

            if (_velocity.X != 0)
            {
                _velocity.X = 0;
            }
        }
        else
        {
            if (_velocity.Y == 0) {
                _velocity.X -= Math.Sign(_velocity.X) * (float)(FRICTION * DeltaT);

                if (Math.Abs(_velocity.X) < (float)(FRICTION * DeltaT))
                {
                    _velocity.X = 0;
                }
            }
            else
            {
                _velocity.X -= Math.Sign(_velocity.X) * (float)(AIR_RESISTANCE * DeltaT);

                if (Math.Abs(_velocity.X) < (float)(AIR_RESISTANCE * DeltaT))
                {
                    _velocity.X = 0;
                }
            }
        }

        ((MovableComponent)_owner.Components.Where(t => t.GetType().Name == "MovableComponent").First()).MoveGameObject(_velocity.X, _velocity.Y);
    }
};