using pixelholdersPlatformer.gameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.states
{
    public class MoveJumpState : IState
    {
        private Player _player;

        public void Enter(Player player)
        {
            _player = player;
        }

        public IState HandleInput(PlayerInput input)
        {
            Vector2 vel = _player.GetPlayerVelocity();

            if (vel.Y >= 0)
            {
                return new FallState();
            }
            else
            {
                if (input == PlayerInput.Left)
                {
                    _player.MovePlayerX(-_player.Speed);
                }
                else if (input == PlayerInput.Right)
                {
                    _player.MovePlayerX(_player.Speed);
                }
                return this;
            }
        }

        public void Update(float timeStep) { }
    }
}
