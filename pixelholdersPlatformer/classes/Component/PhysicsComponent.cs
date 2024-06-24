using pixelholdersPlatformer.classes.gameObjects;
using System.Numerics;

namespace pixelholdersPlatformer.classes.Component;

public class PhysicsComponent : IComponent
{

    private GameObject _owner;

    private const double _gravity = 9.8d;

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
            //TODO: not make it frame dependent
            _velocity.Y += (float)_gravity * 0.016f;
        }
        

        ((MovableComponent)_owner.Components.Where(t => t.GetType().Name == "MovableComponent").First()).MoveGameObject(_velocity.X, _velocity.Y);
        SetVelocityX(0);

    }
};