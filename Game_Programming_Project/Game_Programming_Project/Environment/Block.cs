using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Game_Programming_Project.Sprites;

namespace Game_Programming_Project.Environment
{
    class Block : Sprite
    {

        /*
         * Constructor for non-animated block.
         */
        public Block(Texture2D textureImage, Vector2 position, int collisionOffset, Vector2 speed)
            :base(textureImage, position, collisionOffset, speed)
        {
        }

        /*
         * Constructor to use default frame rate on animated block
         */
        public Block(Texture2D textureImage, Vector2 position, Point frameSize, 
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
        }


        /*
         * Constructor to set framerate on animated block
         */
        public Block(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }

        public override Vector2 direction
        {
            get { return speed; }

        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite based on direction
            position += direction;

            // If sprite is off the screen, move it back within the game window
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > Game.resolution.X - frmSize.X)
                position.X = Game.resolution.X - frmSize.X;
            if (position.Y > Game.resolution.Y - frmSize.Y)
                position.Y = Game.resolution.Y - frmSize.Y;

            base.Update(gameTime, clientBounds);
        }


    }
}
