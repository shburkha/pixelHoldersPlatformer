using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.gameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.behaviours
{
    public class PigBehaviour : IComponent
    {

        private Enemy _owner;
        private Player _player;

        private const float aggroRange = 7;
        private const float attackRangeX = 0.8f;
        private const float attackRangeY = 0.5f;
        private const int _attackCooldown = 250;
        private Stopwatch _attackStopWatch;

        public PigBehaviour(Enemy owner, Player player)
        {
            this._owner = owner;
            this._player = player;
            (_owner.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).SetAttackCooldown(_attackCooldown);
            _attackStopWatch = new Stopwatch();
        }

        bool _attackJustHappened = false;

        //the pig checks for the player. If it is in range it performs an attack
        //the pig first walks to the player
        //we also need to check for ledges so they don't fall
        //by default they randomly walk left and right

        public void Update()
        {

            if ((_owner.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).CurrentAnimationType != managers.AnimationType.Attack)
            {
                float playerCenterX = _player.CoordX + _player.Width / 2;
                float playerCenterY = (_player.CoordY + _player.Height) / 2;

                float ownerCenterX = _owner.CoordX + _owner.Width / 2;
                float ownerCenterY = (_owner.CoordY + _owner.Height) / 2;

                float distanceX = ownerCenterX - playerCenterX;
                float distanceY = ownerCenterY - playerCenterY;
                if (Math.Abs(distanceX) <= aggroRange)
                {
                    //makes an attack
                    if (Math.Abs(distanceX) < attackRangeX && Math.Abs(distanceY) < attackRangeY)
                    {
                        
                        bool isFlipped = (_owner.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).isFlipped;
                        (_owner.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).SetAnimationType(managers.AnimationType.Attack, isFlipped);
                        _attackJustHappened = true;
                        _attackStopWatch.Start();

                    }
                    else
                    {
                        //tries to walk up to player
                        if (distanceX > 0)
                        {
                            ((PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics)).SetVelocityX(-0.1f);
                        }
                        else if (distanceX < 0)
                        {
                            ((PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics)).SetVelocityX(0.1f);
                        }
                    }
                }
                //the attack pushes away and hurts the player
                //_attackCooldown/2 meaning: i want the player to get hit when the sprite shows it
                if (_attackJustHappened && _attackStopWatch.ElapsedMilliseconds > _attackCooldown/2)
                {
                    
                    if (distanceX > 0)
                    {

                        ((PhysicsComponent)_player.GetComponent(gameObjects.Component.Physics)).SetVelocityX(-0.5f);

                    }
                    else if (distanceX < 0)
                    {

                        ((PhysicsComponent)_player.GetComponent(gameObjects.Component.Physics)).SetVelocityX(0.5f);

                    }
                    _attackJustHappened = false;
                    _attackStopWatch.Reset();
                }

            }

            
        }
    }
}
