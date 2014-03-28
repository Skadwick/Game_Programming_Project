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

namespace Game_Programming_Project.Sprites
{
    class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        //Variables for drawing sprites
        SpriteBatch spriteBatch;

        //Player variables
        public static PlayerSprite player;
        public static PlayerSprite playerReference; //Used by other classes to obtain details about the player
        Vector2 playerSpeed = new Vector2(4, 4);

        //Text variables
        SpriteFont generalText;

        //General use variables
        public const float FALLSPEED = 8;

        /*
         * Constructor
         */
        public SpriteManager(Game game)
            : base(game)
        {
            //Child components constructed here
        }



        /*
         * Allows the game component to perform any initialization it needs to before starting
         * to run.  This is where it can query for any required services and load content.
         */
        public override void Initialize()
        {         
            base.Initialize();
        }



        /*
         * 
         */
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //Loading the player
            player = new PlayerSprite(
                Game.Content.Load<Texture2D>(@"Images/Player/idle"),
                new Vector2(200,200), new Point(65,60), 0, new Point(0,0),
                new Point(2, 1), playerSpeed, 256);
            playerReference = player;

            //Loading enemies


            //Loading sounds


            //Loading text
            generalText = Game.Content.Load<SpriteFont>(@"Fonts/Generic");


            base.LoadContent();
        }



        /*
         * 
         */
        public override void Update(GameTime gameTime)
        {

            playerReference = player;
            
            //Player is jumping
            if (player.jumping)
            {
                player.setAnimation(Game.Content.Load<Texture2D>(@"Images/Player/jumping"), 
                    new Point(50,69), new Point(0, 0), new Point(1, 1), 256);
            }

            //Player is attacking
            else if (player.attacking &&
                !player.texture.Equals(Game.Content.Load<Texture2D>(@"Images/Player/attack")))
            {
                player.setAnimation(Game.Content.Load<Texture2D>(@"Images/Player/attack"),
                    new Point(60, 67), new Point(0, 0), new Point(4, 1), 64);
            }

            //Player is standing still (idle)
            else if (player.direction.X == 0 && !player.attacking && !player.jumping &&
                !player.texture.Equals(Game.Content.Load<Texture2D>(@"Images/Player/idle")))
            {
                player.setAnimation(Game.Content.Load<Texture2D>(@"Images/Player/idle"),
                    new Point(65, 60), new Point(0, 0), new Point(2, 1), 180);
            }

            //Player is running to the left or right
            else if (player.direction.X != 0 && !player.attacking &&
                !player.texture.Equals(Game.Content.Load<Texture2D>(@"Images/Player/running")))
            {
                player.setAnimation(Game.Content.Load<Texture2D>(@"Images/Player/running"),
                    new Point(45, 60), new Point(0, 0), new Point(6, 1), 75);
            }

            //Update the player
            player.Update(gameTime, Game.Window.ClientBounds);

            base.Update(gameTime);
        }


        /*
         * 
         */
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // Draw the player
            player.Draw(gameTime, spriteBatch);

            //Draw text
            spriteBatch.DrawString(generalText, 
                "Use the arrow keys or WASD to move." + System.Environment.NewLine +
                "To jump press UP, W, or Space." + System.Environment.NewLine +
                "Hold down E to attack.", new Vector2(10,10), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
