using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.gameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.states
{
    public class AttackState : IState
    {
        private const float _attackCooldown = 0.5f;
        private float _elapsedTime;

        public void Enter(Player player)
        {
            bool isFlipped = (player.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).isFlipped;
            player.PlayAnimation(managers.AnimationType.Attack, isFlipped);
            player.HurtEnemy();
            _elapsedTime = 0; // Reset the elapsed time on entering the state
        }

        public IState HandleInput(PlayerInput input)
        {
            // Only transition to StandState if the cooldown is over
            if (_elapsedTime >= _attackCooldown)
            {
                return new StandState();
            }

            // Stay in the attack state
            return this;
        }

        public void Update(float timeStep)
        {
            // Increment the elapsed time while in the attack state
            _elapsedTime+=timeStep;
        }
    }
}
