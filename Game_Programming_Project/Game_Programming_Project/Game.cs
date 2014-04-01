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


namespace Game_Programming_Project
{
    /*
    This is the main class for our game project.
    */
    public class Game : Microsoft.Xna.Framework.Game
    {

        //Game variables
        public static Vector2 resolution = new Vector2(1024, 576);

        //other variables
        Texture2D background;

        //Default variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Level level;

        /// <summary>
        /// This is the constructor for the game class.  It initializes the graphics device, creates
        /// the content directory, and sets the resolution of the game window.
        /// </summary>        
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)resolution.X;
            graphics.PreferredBackBufferHeight = (int)resolution.Y;
            Content.RootDirectory = "Content";
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
        protected override void Initialize()
        {           
            base.Initialize();
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //background = this.Content.Load<Texture2D>(@"Images/background1");
            level = new Level(Services);
        }



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            level.Update(gameTime, Keyboard.GetState());

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            //spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            level.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
