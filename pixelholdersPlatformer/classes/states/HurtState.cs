using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.managers;
using pixelholdersPlatformer.gameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.states
{
    public class HurtState : IState
    {

        private const float _attackCooldown = 0.2f;
        private float _elapsedTime;
        private Player _player;
        public void Enter(Player player)
        {
            _player = player;
            bool isFlipped = (player.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).isFlipped;
            player.PlayAnimation(AnimationType.Hit, isFlipped);
            player.HurtPlayer();
            AudioManager.Instance.PlaySound("hit");
        }

        public IState HandleInput(PlayerInput input)
        {
            if (_elapsedTime >= _attackCooldown)
            {
                return new StandState();
            }
            

            return this;
        }

        public void Update(float timeStep)
        {
            _elapsedTime += timeStep;
        }
    }
}
