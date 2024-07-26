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

    public interface IState
    {
        IState HandleInput(InputTypes input);
        void Update();
        void Enter(Player player);
    }
}
