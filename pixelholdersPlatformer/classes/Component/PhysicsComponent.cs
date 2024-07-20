using pixelholdersPlatformer.classes.gameObjects;
using System.Numerics;

namespace pixelholdersPlatformer.classes.Component;

public class PhysicsComponent : IComponent
{

    private GameObject _owner;

    private const float _terminalVelocityY = 1;
    public const double GRAVITY = 3.5f;
    public const double AIR_RESISTANCE = 1.5d;
    public const double FRICTION = 2.5d;

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

            //TODO change coordinates to be based on the tilemap
            if (_owner.CoordX != Math.Clamp(_owner.CoordX, 0, 200 - _owner.Width))
            {
                _owner.CoordX = Math.Clamp(_owner.CoordX, 0, 200 - _owner.Width);

                if (Velocity.X != 0)
                {
                    Velocity.X = 0f;
                }
            }
            else
            {
                if (Velocity.Y == 0 || Velocity.Y == (float)(GRAVITY * DeltaT)) {
                    Velocity.X -= Math.Sign(Velocity.X) * (float)(FRICTION * DeltaT);

                    if (Math.Abs(Velocity.X) < (float)(FRICTION * DeltaT))
                    {
                        Velocity.X = 0f;
                    }
                }
                else
                {
                    Velocity.X -= Math.Sign(Velocity.X) * (float)(AIR_RESISTANCE * DeltaT);

                    if (Math.Abs(Velocity.X) < (float)(AIR_RESISTANCE * DeltaT))
                    {
                        Velocity.X = 0f;
                    }
                }
            }
        }
        ((MovableComponent)_owner.GetComponent(gameObjects.Component.Movable)).MoveGameObject(Velocity.X, Velocity.Y);
    }
};