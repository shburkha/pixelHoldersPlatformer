using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.classes.managers;
using pixelholdersPlatformer.classes.states;
using System.Numerics;
using static SDL2.SDL;

namespace pixelholdersPlatformer.gameObjects;

public class Player : GameObject
{

    private float _startPosX;
    private float _startPosY;
    private int _playerHealth;
    private const double _invincibleTime = 1.0d; //in seconds
    private double _timeSinceLastDamage = 0.0d; //in seconds



    public float Speed = 0.25f;
    private IState _state;


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
        _state = new StandState();

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



    private double _timeSinceLastAttack;
    private double _attackCooldown = 150;
    public void HandleInput(PlayerInput input)
    {
        if (input == PlayerInput.Attack && _timeSinceLastAttack >= _attackCooldown)
        {
            IState next = new AttackState();
            _state = next;
            _state.Enter(this);
            _timeSinceLastAttack = 0; // Reset the attack cooldown
        }
        else
        {
            IState next = _state.HandleInput(input);
            if (next != _state)
            {
                _state = next;
                _state.Enter(this);
            }
        }
    }

    public override void Update()
    {
        if (_playerHealth <= 0) { SDL_Quit(); }

        _timeSinceLastDamage += ((PhysicsComponent)GetComponent(Component.Physics)).DeltaT;
        _timeSinceLastAttack += ((PhysicsComponent)GetComponent(Component.Physics)).DeltaT; // Update the attack cooldown timer
        _state.Update((float)((PhysicsComponent)GetComponent(Component.Physics)).DeltaT);
        base.Update();
    }


    public void PlayAnimation(AnimationType animation, bool isFlipped)
    {
        (this.GetComponent(Component.Animatable) as AnimatableComponent).SetAnimationType(animation, isFlipped);
    }

    public Vector2 GetPlayerVelocity()
    {
        return (this.GetComponent(Component.Physics) as PhysicsComponent).Velocity;
    }

}