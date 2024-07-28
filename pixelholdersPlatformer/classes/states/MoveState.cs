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
    public class MoveState : IState
    {
        private Player _player;

        public void Enter(Player player)
        {
            _player = player;
            _player.PlayAnimation(AnimationType.Run);
        }

        public IState HandleInput(PlayerInput input)
        {
            Vector2 vel = _player.GetPlayerVelocity();

            if (input == PlayerInput.Left)
            {
                _player.MovePlayerX(-_player.Speed);
                vel = _player.GetPlayerVelocity();
            }
            else if (input == PlayerInput.Right)
            {
                _player.MovePlayerX(_player.Speed);
                vel = _player.GetPlayerVelocity();
            }

            if (input == PlayerInput.Jump)
            {
                return new JumpState();
            }
            else if (input == PlayerInput.Attack)
            {
                return new AttackState();
            }
            else if (vel.X == 0)
            {
                return new StandState();
            }

            return this;
        }

        public void Update(float timeStep) { }
    }
}
