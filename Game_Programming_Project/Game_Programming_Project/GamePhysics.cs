using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game_Programming_Project
{
    class GamePhysics
    {

        //Constants for verticle movement
        public const float GravityAcceleration = 14; //pixels per second per second (pixes / s^2)


        public static float GetFallSpeed(float yVel, GameTime time)
        {
            return Accelerate(yVel, GravityAcceleration, time);
        }

        public static float Accelerate(float vel, float acc, GameTime time)
        {
            return vel + (acc * (float)time.ElapsedGameTime.Milliseconds/1000);
        }



    }
}
