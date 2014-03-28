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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Game_Programming_Project.Sprites;

namespace Game_Programming_Project.Environment
{

    /// <summary>
    /// This class handles all of the blocks that make up the terrain of the game, the generation of
    /// the terrain, and player collision with the terrain. 
    ///
    /// This class is a DrawableGameComponent, which allows it to...
    /// </summary>
    class EnvironmentManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        SpriteBatch spriteBatch;

        List<Sprite> terrainBlocks = new List<Sprite>();

        Block testBlock;


        /// <summary>
        /// This is the constructor for the EnvironmentManager class.  It simple calls the
        /// Game class's constructor.
        /// </summary> 
        public EnvironmentManager(Game game)
            : base(game)
        {
        }


        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// 
        /// Initilizes all necessary components.  Component classes run in sync with the game
        /// class.  When the game class's Update() and Draw() methods are called, all component
        /// classes also call their corresponding methods.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            /*
             * This is where the terrain for the game is generated. Currently, it simply creates
             * a sort of stair case out of the terrain blocks
             */
            int x = 900;
            int y = 495;
            for(int a = 1; a <= 12; a++)
            {
                for (int b = a; b < 10; b++)
                {
                    x -= 50;
                    terrainBlocks.Add( new Block(Game.Content.Load<Texture2D>(@"Images/Environment/block1"),
                        new Vector2(x, y), 0));
                }
                x = 900;
                y -= 50;
            }
            base.LoadContent();
        }


        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            /* 
             * Handling player collision with terrain blocks
             *
             * collisionSide is used to keep track of where collisions are found.  x = 0 means no horizontal collision.
             * x = 1 means there was a collision on the left side of the block.  x = -1 means there was a
             * collision on the right side of the block. y = 0 means there was no verticle collision.  y = 1 means there
             * was a collision on the top of the block.  y = -1 means there was collision on the bottom of the block.
             */
            Vector2 collisionSide = new Vector2(0, 0);
            foreach(Sprite tb in terrainBlocks) //Loop through the list of blocks in use
            {
                tb.Update(gameTime, Game.Window.ClientBounds); //Update each block

                //Check if the player collided with a block
                if (SpriteManager.player.collisionRect.Intersects(tb.collisionRect))
                {
                    
                    //Player hit the top of the terrain block
                    if ((SpriteManager.player.pos.Y >= (tb.pos.Y - SpriteManager.player.frameSize.Y)) &&
                        (SpriteManager.player.pos.Y <= (tb.pos.Y - SpriteManager.player.frameSize.Y + SpriteManager.FALLSPEED)))
                    {
                        collisionSide.Y = 1;
                    }

                    //Player hit the left side of the terrain block
                    else if (SpriteManager.player.pos.X >= (tb.pos.X - SpriteManager.player.frameSize.X) &&
                        SpriteManager.player.direction.X > 0)
                    {
                        collisionSide.X = -1;
                    }

                    //Player hit the right side of the terrain block
                    else if (SpriteManager.player.pos.X <= (tb.pos.X + tb.frameSize.X) &&
                        SpriteManager.player.direction.X < 0)
                    {
                        collisionSide.X = 1;
                    }
                }

            }

            //Notify the sprite manager where the collision occured
            SpriteManager.player.collisionLocation = collisionSide;
            
            base.Update(gameTime);
        }



        /*
         * Drawing each of the terrain blocks to the screen
         */
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            foreach (Sprite tb in terrainBlocks) //Loop through each block, and draw it
            {
                tb.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
