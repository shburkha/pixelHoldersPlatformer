using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.classes.managers;
using static SDL2.SDL;

namespace pixelholdersPlatformer.gameObjects;

public class Player : GameObject
{

    private float _startPosX;
    private float _startPosY;
    private int _playerHealth;
    private const double _invincibleTime = 1.0d; //in seconds
    private double _timeSinceLastDamage = 0.0d; //in seconds
    public Player(float coordX, float coordY, float width, float height) : base(coordX, coordY, width, height)
    {
        this.Components.Add(new MovableComponent(this));
        this.Components.Add(new PhysicsComponent(this));
        this.Components.Add(new CollisionComponent(this));
        this.Components.Add(new AnimatableComponent(this, "01-King Human"));
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

    public void PlayerToCheckpoint()
    {
        // TODO check current level and set checkpoint accordingly
        // CoordX = 103;
        // CoordY = 10;

        CoordX = 198;
        CoordY = 0;
    }


    public void HurtPlayer()
    {
        if (_timeSinceLastDamage <= _invincibleTime) { return; }
        _playerHealth -= 1;
        _timeSinceLastDamage = 0;
    }

    public void SetDeltaTime(double dt)
    {
        ((PhysicsComponent)GetComponent(Component.Physics)).DeltaT = dt;
    }

    public override void Update()
    {
        if (_playerHealth <= 0) { UIManager.Instance.ChangeScene(Scene.GameOver); }

        _timeSinceLastDamage += ((PhysicsComponent)GetComponent(Component.Physics)).DeltaT;

        base.Update();
    }
}