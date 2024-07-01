using pixelholdersPlatformer.classes.gameObjects;
using System.Numerics;

namespace pixelholdersPlatformer.classes.Component;

public class PhysicsComponent : IComponent
{

    private GameObject _owner;

    private const double _gravity = 4d;
    private const float _terminalVelocityY = 1;

    public Vector2 Velocity;

    public PhysicsComponent(GameObject owner)
    {
        _owner = owner;
        IsFallable = false;
    }

    public void SetVelocity(float amountX, float amountY)
    {
        Velocity.X = amountX;
        Velocity.Y = amountY;

    }

    public void SetVelocityX(float amountX)
    {
        Velocity.X = amountX;


    }
    public void SetVelocityY(float amountY)
    {
        Velocity.Y = amountY;

    }

    //A player is Fallable, but a platform is not
    public bool IsFallable { get; set; }

   
    public void Update()
    {
        if (IsFallable)
        {
            if (_owner.CoordY >= 100 - _owner.Height)
            {
                _owner.CoordY = 100 - _owner.Height;
                if (Velocity.Y > 0)
                {
                    Velocity.Y = 0;
                }

            }
            else
            {
                //TODO: not make it frame dependent
                if(Velocity.Y < _terminalVelocityY) 
                
                {
                    Velocity.Y += (float)_gravity * 0.016f;
                }
                

            }
        }
        
        ((MovableComponent)_owner.Components.Where(t => t.GetType().Name == "MovableComponent").First()).MoveGameObject(Velocity.X, Velocity.Y);
        SetVelocityX(0);

    }
};