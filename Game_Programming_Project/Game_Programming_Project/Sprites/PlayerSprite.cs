/*
 * @Authors
 * <Joshua Shadwick> (Joshua.Shadwick@bobcats.gcsu.edu)
 * <Kasey Dean> (Kasey.Dean@bobcats.gcsu.edu)
 * <Robert Strand> (robert.strand@bobcats.gcsu.edu)
 *
 * @Overview
 * This video game was created by Joshua Shadwick, Kasey Dean, and Robert Strand for 
 * CSCI 4950 - Game Programming, instructed by Dr. Jenq-Foung (JF) Yao during the
 * Spring 2014 semester at Georgia College and State University.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Programming_Project.Sprites
{
    class PlayerSprite: Sprite
    {

        //Speed in the Y direction that the player can jump
        float jumpSpeed = 5;

        //Max jump distance
        float maxJump = 120;

        //Keeps track of the max coordinate the player can jump to
        float curMaxJumpHeight = 0;

        //Keeps track of where the player hits terrain, and prevents them from moving that direction
        public Vector2 collisionLocation = Vector2.Zero;

        //True when the player is jumping in the air
        public bool jumping = false;

        //True when the player is attacking
        public bool attacking = false;


        /*
         * Constructor to use default frame rate
         */
        public PlayerSprite(Texture2D textureImage, Vector2 position, Point frameSize, 
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
        }


        /*
         * Constructor to set framerate
         */
        public PlayerSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }


        /*
         * Get direction of sprite based on player input and speed
         */
        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;

                //Check if player is moving left
                if ((Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A)) &&
                    collisionLocation.X != 1)
                {
                    inputDirection.X -= 1;
                }

                //Check if player is moving right
                if ( (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D)) &&
                    collisionLocation.X != -1)
                {
                    inputDirection.X += 1;
                }

                return inputDirection * speed;
            }
        }



        /*
         * Logic to update various aspects of the player, when needed.
         */
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

            //Flips the player's sprite depending on if they are moving to the left or the right
            if (direction.X < 0)
                spriteEffect = SpriteEffects.FlipHorizontally;
            else if (direction.X > 0)
                spriteEffect = SpriteEffects.None;

            //Allow the player to jump
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W) ||
                Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //Only allow the player to jump if they are standing on the ground
                if (position.Y == (Game.resolution.Y - frmSize.Y) || collisionLocation.Y == 1)
                {
                    jumping = true;
                    curMaxJumpHeight = position.Y - maxJump;
                }
            }
            else //Stop jumping when the key is released
            {
                jumping = false;
                curMaxJumpHeight = 0;
            }

            //Simulate gravity when not jumping, when reach max jump height, or when colliding with
            //the bottom of a terrain block.
            if ((!jumping || (jumping && position.Y <= curMaxJumpHeight) || 
                (jumping && position.Y <= 0) ) && collisionLocation.Y != 1)
            {
                if (position.Y < clientBounds.Height - frmSize.Y)
                {
                    position.Y += SpriteManager.FALLSPEED;
                    jumping = false;
                    curMaxJumpHeight = 0;
                }
            }
            //Otherwise, allow the player to jump when jumping is true
            else if(jumping)
            {
                position.Y -= jumpSpeed;
            }


            //Allow the player to attack
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                attacking = true;
            }
            else
            {
                attacking = false;
            }

            //Reset the collision location
            collisionLocation = Vector2.Zero;

            base.Update(gameTime, clientBounds);
        }
    }
}
