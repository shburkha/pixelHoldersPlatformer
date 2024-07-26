using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.classes.managers;
using pixelholdersPlatformer.gameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.states
{
    public class JumpState : IState
    {
        private Player _player;
        public void Enter(Player player)
        {
            _player = player;
            if((_player.GetComponent(gameObjects.Component.Physics) as PhysicsComponent).Velocity.Y == 0)
            {
                _player.MovePlayerY(-0.5f);
            }
            (player.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).CurrentAnimationType = AnimationType.Jump;
        }

        public IState HandleInput(InputTypes input)
        {
            switch (input)
            {
                case InputTypes.PlayerLeft:
                    return new RunState();

                case InputTypes.PlayerRight:
                    return new RunState();

                case InputTypes.PlayerJump:
                    if ((_player.GetComponent(gameObjects.Component.Physics) as PhysicsComponent).Velocity.Y == 0)
                    {
                        return new StandState();
                    }
                    else
                    {
                        return new FallState();
                    }

                case InputTypes.PlayerAttack:
                    if ((_player.GetComponent(gameObjects.Component.Physics) as PhysicsComponent).Velocity.Y == 0)
                    {
                        return new AttackState();
                    }
                    break;
            }
            return this;
        }

        public void Update()
        {

        }
    }
}
