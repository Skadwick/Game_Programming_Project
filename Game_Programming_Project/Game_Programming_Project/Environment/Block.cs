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
using Game_Programming_Project.Sprites;

namespace Game_Programming_Project.Environment
{   
    /// <summary>
    /// This class provides the functionality for static blocks which will be used in the 
    /// creation of the terrain of the game.  Typically, all that is needed to create a block
    /// is a texture, position, and collision offset.  However, constructors for animated blocks
    /// have also been added.
    /// </summary>
    class Block : Sprite
    {

        private bool collide;


        /*
         * Constructor for non-animated block. Automatically sets block speed to zero
         */
        public Block(Texture2D textureImage, Vector2 position, int collisionOffset)
            :base(textureImage, position, collisionOffset, Vector2.Zero)
        {
            collide = true;
        }


        /*
         * Constructor to use default frame rate on animated block. 
         * Automatically sets block speed to zero.
         */
        public Block(Texture2D textureImage, Vector2 position, Point frameSize, 
            int collisionOffset, Point currentFrame, Point sheetSize)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, Vector2.Zero)
        {
            collide = true;
        }


        /*
         * Constructor to set framerate on animated block. Automatically sets block speed to zero.
         */
        public Block(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, 
            Point sheetSize, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, Vector2.Zero, millisecondsPerFrame)
        {
            collide = true;
        }


        /*
         * Accessor and mutator for the collide property
         */
        public bool allowCollision
        {
            get { return collide; }
            set { collide = value; }
        }


    }
}
