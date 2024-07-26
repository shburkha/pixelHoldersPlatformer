using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.classes.managers;
using pixelholdersPlatformer.gameObjects;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.states
{
    internal class RunState : IState
    {
        private Player _player;
        public void Enter(Player player)
        {
            _player = player;
            (player.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).CurrentAnimationType = AnimationType.Run;
        }

        public IState HandleInput(InputTypes input)
        {
            switch (input)
            {
                case InputTypes.PlayerLeft:
                    _player.MovePlayerX(-0.22f); 
                    break;
                case InputTypes.PlayerRight:
                    _player.MovePlayerX(0.22f);
                    break;

                case InputTypes.PlayerJump:
                    return new JumpState();

                case InputTypes.PlayerAttack:
                    return new AttackState();
            }
            return this;
        }

        public void Update()
        {
            
        }
    }
}
