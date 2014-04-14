using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Programming_Project
{
    class Camera
    {

        private static Camera instance;
        Vector2 position;
        Matrix viewMatrix;

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        public static Camera Instance
        {
            get
            {
                if (instance == null)
                    instance = new Camera();
                return instance;
            }
        }

        //The camera follows the player (focal position); the player will always be at the center of the screen in terms of
        //x coordinates.
        public void SetFocalPoint(Vector2 focalPosition)
        {
            position = new Vector2(focalPosition.X - Game.resolution.X / 2,
                focalPosition.Y - Game.resolution.Y / 2);

            //Camera stays withing the boundaries of the level
            if (position.X < 0)
                position.X = 0;
            if (position.Y > 0)
                position.Y = 0;
            if (position.X + 1024 > 128 * 32)
                position.X = (128 * 32) - 1024;
        }

        public void Update()
        {
            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }

    }
}
