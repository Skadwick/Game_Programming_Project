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
        public const float GravityAcceleration = 14;


        public static float GetFallSpeed(float yVel, GameTime time)
        {
            return yVel + (GravityAcceleration * ((float)time.ElapsedGameTime.Milliseconds)/1000);
        }



    }
}
