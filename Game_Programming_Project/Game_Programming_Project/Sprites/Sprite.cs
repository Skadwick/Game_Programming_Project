using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Programming_Project.Sprites
{
    abstract class Sprite
    {
        //Variables needed to draw the sprite
        Texture2D textureImage;
        protected Point frmSize;
        Point curFrame;
        Point shtSize;
        protected SpriteEffects spriteEffect = SpriteEffects.None;

        //Collision variables
        int collisionOffset;

        //Framerate variables
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 16;

        //Movement variables
        protected Vector2 speed;
        protected Vector2 position;

        //Abstract definition of direction property
        public virtual Vector2 direction
        {
            get { return speed * position; }
        }

        /*
         * Main Sprite class constructor. Sets all the important variables which
         * will be used within this class.
         */
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frmSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.curFrame = currentFrame;
            this.shtSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }



        /*
         * Calls the main sprite constructor and passes defaultMillisecondsPerFrame as the
         * millisecondsPerFrame argument.
         */
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }


        /*
         * Constructor for sprites that are not animated
         */
        public Sprite(Texture2D textureImage, Vector2 position, int collisionOffset, Vector2 speed)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.collisionOffset = collisionOffset;
            this.speed = speed;

            this.curFrame = new Point(0,0);
            this.shtSize = new Point(1, 1);
            this.frmSize.X = textureImage.Width;
            this.frmSize.Y = textureImage.Height;
            this.millisecondsPerFrame = defaultMillisecondsPerFrame;

        }



        /*
         * 
         */
        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //Check to see if it is time to update frame
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                // Increment to next frame
                timeSinceLastFrame = 0;
                ++curFrame.X;
                if (curFrame.X >= shtSize.X)
                {
                    curFrame.X = 0;
                    ++curFrame.Y;
                    if (curFrame.Y >= shtSize.Y)
                        curFrame.Y = 0;
                }
            }
        }



        /*
         * Draws the current frame of the sprite 
         */
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw the sprite
            spriteBatch.Draw(textureImage, position,
                new Rectangle(curFrame.X * frmSize.X,
                    curFrame.Y * frmSize.Y, frmSize.X, frmSize.Y),
                Color.White, 0, Vector2.Zero,
                1f, spriteEffect, 0);
        }


        /*
         * Allows the sprite's sprite sheet to be updated, which 
         * allows for changes in animation.
         */
        public virtual void setAnimation(Texture2D txt, Point frmSz, Point curFrm, Point shtSz, int speed)
        {
            textureImage = txt;
            frmSize = frmSz;
            curFrame = curFrm;
            shtSize = shtSz;
            millisecondsPerFrame = speed;
        }



        /*
         * Uses sprite position and frame size in order to create a rectangle which
         * will be used to detect collision with another sprite
         */
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frmSize.X - (collisionOffset * 2),
                    frmSize.Y - (collisionOffset * 2));
            }
        }


        /*
         * Accessors and mutators
         */
        public virtual Texture2D texture
        {
            get { return textureImage; }
            set { textureImage = value; }
        }

        public virtual Point frameSize
        {
            get { return frmSize; }
            set { frmSize = value; }
        }

        public virtual Point currentFrame
        {
            get { return curFrame; }
            set { curFrame = value; }
        }

        public virtual Point sheetSize
        {
            get { return shtSize; }
            set { shtSize = value; }
        }

        public virtual Vector2 pos
        {
            get { return position; }
            set { position = value; }
        }

    }
}
