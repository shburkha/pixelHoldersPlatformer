using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pixelholdersPlatformer.gameObjects;

namespace pixelholdersPlatformer.classes.states
{



    //maybe this is redundant but I wanted to make a separate enum for playerInputs
    public enum PlayerInput
    { 
        Left, Right, Jump, Attack, None, MoveJumpLeft, MoveJumpRight, MoveFallLeft, MoveFallRight
    }


    public interface IState
    {
        IState HandleInput(PlayerInput input);
        //this is only used if you need to track something like by crouching you charge a superattack or looking for cooldowns
        void Update(float timeStep);
        //this is where we put the code where the animation begins or the appropriate sound effect is called
        void Enter(Player player);
    }
}
