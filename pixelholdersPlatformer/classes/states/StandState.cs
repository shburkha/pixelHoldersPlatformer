using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.managers;
using pixelholdersPlatformer.gameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.states
{
    public class StandState : IState
    {
        private Player _player;

        public void Enter(Player player)
        {
            _player = player;
            bool isFlipped = (_player.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).isFlipped;
            _player.PlayAnimation(AnimationType.Idle, isFlipped);
        }

        public IState HandleInput(PlayerInput input)
        {
            if (input == PlayerInput.Jump)
            {
                return new JumpState();
            }
            else if (input == PlayerInput.Left || input == PlayerInput.Right)
            {
                return new MoveState();
            }
            else if (input == PlayerInput.Attack)
            {
                return new AttackState();
            }
            else if (input == PlayerInput.Hurt)
            { 
                return new HurtState();
            }
            else
            {
                return this;
            }
        }

        public void Update(float timeStep) { }
    }
}
