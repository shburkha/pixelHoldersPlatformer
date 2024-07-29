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
    public int PlayerHealth;
    private const double _invincibleTime = 1.0d; //in seconds
    private double _timeSinceLastDamage = 0.0d; //in seconds

    private List<GameObject> _enemies;


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
        PlayerHealth = 3;
        _state = new StandState();

    }


    public void SetEnemies(List<GameObject> enemies)
    {
        _enemies = enemies;
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
        // this is to checkpoint
        CoordX = 103;
        CoordY = 13;

        // this is to finish
        // CoordX = 198;
        // CoordY = 0;
    }
    public void HurtPlayer()
    {
        if (_timeSinceLastDamage <= _invincibleTime) { return; }
        PlayerHealth -= 1;
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


    private const float hitRange = 2f;
    public void HurtEnemy()
    {

        foreach (Enemy enemy in _enemies)
        {
            float distanceX = (enemy.CoordX + enemy.Width/2) - (this.CoordX + this.Width/2);
            float distanceY = (enemy.CoordY + enemy.Height / 2) - (this.CoordY + this.Height / 2);
            if (Math.Abs(distanceX) < hitRange && Math.Abs(distanceY) < hitRange && IsThePlayerFacingTheEnemy(enemy, distanceX)) 
            { 
                enemy.IsHurt = true;
            }
        }
    }

    private bool IsThePlayerFacingTheEnemy(Enemy enemy, float distanceX)
    {
        AnimatableComponent pComp = GetComponent(Component.Animatable) as AnimatableComponent;
        AnimatableComponent eComp = enemy.GetComponent(Component.Animatable) as AnimatableComponent;

        //the player is to the left of the enemy
        if (distanceX > 0)
        {
            return !pComp.isFlipped;
        }
        //the player is to the right of the enemy
        else
        {
            return pComp.isFlipped;
        }
    }

    public override void Update()
    {
        if (PlayerHealth <= 0) { UIManager.Instance.ChangeScene(Scene.GameOver); }

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