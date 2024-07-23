﻿using pixelholdersPlatformer.classes.Component;
using pixelholdersPlatformer.classes.gameObjects;
using pixelholdersPlatformer.gameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pixelholdersPlatformer.classes.behaviours
{
    public class PigBehaviour : IBehaviour
    {

        private Enemy _owner;
        private Player _player;
        private List<GameObject> _collidableObjects;

        private const float aggroRange = 7;
        private const float attackRangeX = 0.8f;
        private const float attackRangeY = 0.5f;
        private const int _attackCooldown = 250;

        //for debugging reasons

        public float futureCenterPosX;
        public float futureCenterPosY;

        private Stopwatch _attackStopWatch;

        public PigBehaviour(Enemy owner, Player player, List<GameObject> collidableObjects)
        {
            this._owner = owner;
            this._player = player;
            (_owner.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).SetAttackCooldown(_attackCooldown);
            _attackStopWatch = new Stopwatch();
            _collidableObjects = collidableObjects;
        }

        bool _attackJustHappened = false;

        //the pig checks for the player. If it is in range it performs an attack
        //the pig first walks to the player
        //we also need to check for ledges so they don't fall
        //by default they randomly walk left and right

        public void Update()
        {

            if ((_owner.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).CurrentAnimationType != managers.AnimationType.Attack)
            {
                float playerCenterX = _player.CoordX + _player.Width / 2;
                float playerCenterY = (_player.CoordY + _player.Height) / 2;

                float ownerCenterX = _owner.CoordX + _owner.Width / 2;
                float ownerCenterY = _owner.CoordY + _owner.Height / 2;

                float distanceX = ownerCenterX - playerCenterX;
                float distanceY = ownerCenterY - playerCenterY;
                if (Math.Abs(distanceX) <= aggroRange)
                {
                    //makes an attack
                    if (Math.Abs(distanceX) < attackRangeX && Math.Abs(distanceY) < attackRangeY)
                    {
                        
                        bool isFlipped = (_owner.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).isFlipped;
                        (_owner.GetComponent(gameObjects.Component.Animatable) as AnimatableComponent).SetAnimationType(managers.AnimationType.Attack, isFlipped);
                        _attackJustHappened = true;
                        _attackStopWatch.Start();

                    }
                    else
                    {
                        //tries to walk up to player but only if there is no ledge
                        //how to check for ledge: the Enemy somehow needs to look in front AND under itself
                        //idea2: using rays...
                        //then we check if there is something under, where we still need all the gameobjects...


                        bool wouldItFallNextTime = true;
                        int direction = Math.Sign((_owner.GetComponent(gameObjects.Component.Physics) as PhysicsComponent).Velocity.X);
                        if (direction == 0)
                        {
                            direction = -1;
                        }
                        //b is the looking radius
                        float b  = 3;
                        int angleOfLooking = 45;
                        double angleOfLookingInRadian = (angleOfLooking * (Math.PI/180));

                        //futureCenterPosX = (float)(ownerCenterX + b * Math.Sin(angleOfLookingInRadian));
                        //futureCenterPosY = (float)(ownerCenterY + b * Math.Cos(angleOfLookingInRadian));

                        foreach (GameObject collidible in _collidableObjects) 
                        {
                            for (int i = 1; i < b; i++)
                            {
                                futureCenterPosX = (float)(ownerCenterX + i * Math.Sin(angleOfLookingInRadian));
                                futureCenterPosY = (float)(ownerCenterY + i * Math.Cos(angleOfLookingInRadian));
                                if (futureCenterPosX > collidible.CoordX &&
                                futureCenterPosX < collidible.CoordX + collidible.Width &&
                                futureCenterPosY > collidible.CoordY &&
                                futureCenterPosY < collidible.CoordY + collidible.Height)
                                {
                                    //Collision happens
                                    wouldItFallNextTime = false;
                                    break;

                                }
                                if (!wouldItFallNextTime)
                                {
                                     break;
                                }

                            }

                            

                        }

                        if (!wouldItFallNextTime)
                        {
                            if (distanceX > 0)
                            {
                                ((PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics)).SetVelocityX(-0.1f);
                            }
                            else if (distanceX < 0)
                            {
                                ((PhysicsComponent)_owner.GetComponent(gameObjects.Component.Physics)).SetVelocityX(0.1f);
                            }
                        }                       
                    }
                }
                //the attack pushes away and hurts the player TODO: implement player getting hurt
                //_attackCooldown/2 meaning: i want the player to get hit when the sprite shows it
                if (_attackJustHappened && _attackStopWatch.ElapsedMilliseconds > _attackCooldown/2)
                {
                    
                    if (distanceX > 0)
                    {

                        ((PhysicsComponent)_player.GetComponent(gameObjects.Component.Physics)).SetVelocityX(-0.5f);

                    }
                    else if (distanceX < 0)
                    {

                        ((PhysicsComponent)_player.GetComponent(gameObjects.Component.Physics)).SetVelocityX(0.5f);

                    }
                    ((PhysicsComponent)_player.GetComponent(gameObjects.Component.Physics)).SetVelocityY(-0.2f);
                    _attackJustHappened = false;
                    _attackStopWatch.Reset();
                }
            } 
        }
    }
}
