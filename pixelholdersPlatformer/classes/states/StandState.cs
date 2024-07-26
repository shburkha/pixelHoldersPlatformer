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
    public class StandState : IState
    {
        public void Enter(Player player)
        {
            (player.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).CurrentAnimationType = AnimationType.Idle;
        }

        public IState HandleInput(InputTypes input)
        {
            switch(input)
            {
                case InputTypes.PlayerLeft:
                    return new RunState();
                
                case InputTypes.PlayerRight:
                    return new RunState();

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
