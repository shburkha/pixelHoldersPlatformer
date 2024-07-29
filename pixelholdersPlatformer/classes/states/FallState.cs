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
    public class FallState : IState
    {
        private Player _player;

        public void Enter(Player player)
        {
            _player = player;
            bool isFlipped = (_player.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).isFlipped;
            _player.PlayAnimation(AnimationType.Fall, isFlipped);
        }

        public IState HandleInput(PlayerInput input)
        {
            Vector2 vel = _player.GetPlayerVelocity();

            if (input == PlayerInput.Left || input == PlayerInput.Right)
            {
                return new MoveFallState();
            }

            if (vel.Y == 0)
            {
                return new StandState();
            }

            return this;
        }

        public void Update(float timeStep) { }
    }
}
