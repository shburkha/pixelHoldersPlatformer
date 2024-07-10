using pixelholdersPlatformer.classes.gameObjects;
using System.Numerics;

namespace pixelholdersPlatformer.classes.Component;

public class PhysicsComponent : IComponent
{

    private GameObject _owner;

    private const float _terminalVelocityY = 1;
    private const double GRAVITY = 5.5d;
    private const double AIR_RESISTANCE = 1.5d;
    private const double FRICTION = 5.0d;

    public double DeltaT = 0.016d;

    public Vector2 Velocity;

    public PhysicsComponent(GameObject owner)
    {
        _owner = owner;
        HasGravity = false;
        CanMove = false;
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

    //A player has gravity, but a platform does not
    public bool HasGravity { get; set; }

    public bool CanMove { get; set; }

    public void Update()
    {
        if (HasGravity)
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
                Velocity.Y += (float)(GRAVITY * DeltaT);
            }

            if (_owner.CoordX != Math.Clamp(_owner.CoordX, 0, 100 - _owner.Width))
            {
                _owner.CoordX = Math.Clamp(_owner.CoordX, 0, 100 - _owner.Width);

                if (Velocity.X != 0)
                {
                    Velocity.X = 0;
                }
            }
            else
            {
                if (Velocity.Y == 0) {
                    Velocity.X -= Math.Sign(Velocity.X) * (float)(FRICTION * DeltaT);

                    if (Math.Abs(Velocity.X) < (float)(FRICTION * DeltaT))
                    {
                        Velocity.X = 0;
                    }
                }
                else
                {
                    Velocity.X -= Math.Sign(Velocity.X) * (float)(AIR_RESISTANCE * DeltaT);

                    if (Math.Abs(Velocity.X) < (float)(AIR_RESISTANCE * DeltaT))
                    {
                        Velocity.X = 0;
                    }
                }
            }
        }
        ((MovableComponent)_owner.Components.Where(t => t.GetType().Name == "MovableComponent").First()).MoveGameObject(Velocity.X, Velocity.Y);
        //SetVelocityX(0);

    }
};