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
            AudioManager.Instance.StartRunning();
        }

        public IState HandleInput(PlayerInput input)
        {

            if (input == PlayerInput.Left)
            {
                _player.MovePlayerX(-_player.Speed);
                _player.PlayAnimation(AnimationType.Run, true);
                
            }
            else if (input == PlayerInput.Right)
            {
                _player.MovePlayerX(_player.Speed);
                _player.PlayAnimation(AnimationType.Run, false);
                
            }
            else if (input == PlayerInput.Jump)
            {
                AudioManager.Instance.StopRunning();
                return new JumpState();
            }

            else if (input == PlayerInput.Attack)
            {
                AudioManager.Instance.StopRunning();
                return new AttackState();
            }

            if (_player.GetPlayerVelocity().Y > 0)
            {
                AudioManager.Instance.StopRunning();
                return new FallState();
            }

            else if (input == PlayerInput.None)
            {
                AudioManager.Instance.StopRunning();
                return new StandState();
            }





            return this;
            
        }

        public void Update(float timeStep) { }
    }
}
