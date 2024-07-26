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
    public class FallState : IState
    {
        public void Enter(Player player)
        {
            (player.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).CurrentAnimationType = AnimationType.Fall;
        }

        public IState HandleInput(InputTypes input)
        {
            switch (input)
            {
                case InputTypes.PlayerLeft:
                    return new RunState();

                case InputTypes.PlayerRight:
                    return new RunState();

                default:
                    return new RunState();
            }
        }

        public void Update()
        {

        }
    }
}
