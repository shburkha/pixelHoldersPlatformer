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
    public class JumpState : IState
    {
        private Player _player;

        public void Enter(Player player)
        {
            _player = player;
            _player.PlayAnimation(AnimationType.Jump);
            _player.MovePlayerY(-0.5f); // Initial jump impulse
        }

        public IState HandleInput(PlayerInput input)
        {
            Vector2 vel = _player.GetPlayerVelocity();

            if (vel.Y < 0)
            {
                if (input == PlayerInput.Left || input == PlayerInput.Right)
                {
                    return new MoveJumpState();
                }
                return this;
            }
            else if (vel.Y > 0)
            {
                return new FallState();
            }

            return this;
        }

        public void Update(float timeStep) { }
    }
}
