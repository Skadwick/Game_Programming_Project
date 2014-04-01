using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game_Programming_Project
{
    class GameMath
    {

        //Finds where two rectangles collide, and determines where the collision occured
        public static Vector2 CollisionDepth(Rectangle rec1, Rectangle rec2)
        {
            //If there is no intersection, then there is no intersection depth.
            if (!rec1.Intersects(rec2))
                return Vector2.Zero;

            Rectangle intersectionArea = Rectangle.Intersect(rec1, rec2);
            Vector2 depth = new Vector2(intersectionArea.Width, intersectionArea.Height);

            if (rec1.X < rec2.X)
                depth.X *= -1;
            if (rec1.Y < rec2.Y)
                depth.Y *= -1;

            return depth;
        }

    }
}
