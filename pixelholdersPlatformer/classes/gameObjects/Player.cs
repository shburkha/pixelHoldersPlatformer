using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;

namespace pixelholdersPlatformer.gameObjects;

public class Player : GameObject
{

    private float _startPosX;
    private float _startPosY;
    private int _playerHealth;
    public Player(float coordX, float coordY, float width, float height) : base(coordX, coordY, width, height)
    {
        this.Components.Add(new MovableComponent(this));
        this.Components.Add(new PhysicsComponent(this));
        this.Components.Add(new CollisionComponent(this));
        this.Components.Add(new AnimatableComponent(this, "03-Pig"));
        ((PhysicsComponent)GetComponent(Component.Physics)).HasGravity = true;
        ((PhysicsComponent)GetComponent(Component.Physics)).CanMove = true;
        _startPosX = coordX;
        _startPosY = coordY;
        _playerHealth = 3;

    }

    public void MovePlayer(float amountX, float amountY)
    { 
        ((PhysicsComponent)GetComponent(Component.Physics)).SetVelocity(amountX, amountY);
    }
    public void MovePlayerX(float amountX)
    {
        ((PhysicsComponent)GetComponent(Component.Physics)).SetVelocityX(amountX);
    }
    public void MovePlayerY(float amountY)
    {
        ((PhysicsComponent)GetComponent(Component.Physics)).SetVelocityY(amountY);
    }

    public void ResetPlayerPosition()
    {
        CoordX = _startPosX;
        CoordY = _startPosY;
    }


    public void HurtPlayer()
    {
        _playerHealth -= 1;
    }

    public void SetDeltaTime(double dt)
    {
        ((PhysicsComponent)GetComponent(Component.Physics)).DeltaT = dt;
    }
}