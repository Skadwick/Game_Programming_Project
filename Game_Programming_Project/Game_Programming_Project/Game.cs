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
using Game_Programming_Project.Interface;


namespace Game_Programming_Project
{
    /*
    This is the main class for our game project.
    */
    public class Game : Microsoft.Xna.Framework.Game
    {

        //Game variables
        public static Vector2 resolution = new Vector2(1024, 576);
        GameState gameState;
        public int levelIndex;
        public int numLevels = 2;

        //Cursor variables
        Texture2D cursorTexture;
        Vector2 cursorPos = Vector2.Zero;
        MouseState previousMouseState;

        //Interface variables
        PlayerHud hud;
        Button play;
        Button instructions;
        Button continuePlaying;
        Button playAgain;
        Button quit;

        //other variables
        Texture2D startBackground;
        Texture2D background;
        Texture2D gameOverBackground;
        Texture2D pausedBackground;
        int lastPause = 0;
        const int PauseDelay = 333;

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

            //Loading backgrounds
            background = this.Content.Load<Texture2D>(@"backgrounds/back_city");
            startBackground = this.Content.Load<Texture2D>(@"backgrounds/back_start");
            gameOverBackground = this.Content.Load<Texture2D>(@"backgrounds/back_gameover");
            pausedBackground = this.Content.Load<Texture2D>(@"backgrounds/back_paused");

            //Loading interface elements
            cursorTexture = this.Content.Load<Texture2D>(@"Interface/Cursors/cursor");
            hud = new PlayerHud(Services);
            play = new Button(this.Content.Load<Texture2D>(@"Interface/Buttons/btn_play"),
                this.Content.Load<Texture2D>(@"Interface/Buttons/btn_playhvr"), new Vector2(362, 250));
            instructions = new Button(this.Content.Load<Texture2D>(@"Interface/Buttons/btn_instructions"),
                this.Content.Load<Texture2D>(@"Interface/Buttons/btn_instructionshvr"), new Vector2(362, 350)); ;
            playAgain = new Button(this.Content.Load<Texture2D>(@"Interface/Buttons/btn_playagain"),
                this.Content.Load<Texture2D>(@"Interface/Buttons/btn_playagainhvr"), new Vector2(362, 250)); ;
            continuePlaying = new Button(this.Content.Load<Texture2D>(@"Interface/Buttons/btn_continueplaying"),
                this.Content.Load<Texture2D>(@"Interface/Buttons/btn_continueplayinghvr"), new Vector2(362, 250)); ;
            quit = new Button(this.Content.Load<Texture2D>(@"Interface/Buttons/btn_quit"),
                this.Content.Load<Texture2D>(@"Interface/Buttons/btn_quithvr"), new Vector2(362, 450)); ;

            //Setting initial game state and level index
            gameState = GameState.Start;
            levelIndex = 1;
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            //Update the cursor position
            if (mouseState.X != previousMouseState.X || mouseState.Y != previousMouseState.Y)//updates cursor position
            {
                cursorPos = new Vector2(mouseState.X, mouseState.Y);
            }
            previousMouseState = mouseState;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Update the game based on which state the game is currently in
            switch (gameState)
            {
                case GameState.Start:
                    Start(gameTime, mouseState);
                    break;

                case GameState.Playing:
                    Playing(gameTime, keyboardState);
                    break;

                case GameState.Paused:
                    Paused(gameTime, mouseState, keyboardState);
                    break;

                case GameState.GameOver:
                    GameOver(gameTime, mouseState);
                    break;
                    
                default:
                    throw new Exception("Invalid GameState.");
            }           

            base.Update(gameTime);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void Start(GameTime gameTime, MouseState mouseState)
        {
            //Update button positions
            play.Position = new Vector2(100, 340);
            instructions.Position = new Vector2(100, 410);
            quit.Position = new Vector2(100, 480);

            //Check if buttons are being clicked, and update them
            if (play.Update(gameTime, mouseState))
            {
                gameState = GameState.Playing;
                levelIndex = 1;
                level = new Level(Services, levelIndex);
            }

            if (instructions.Update(gameTime, mouseState))
            {
                
            }

            if (quit.Update(gameTime, mouseState))
            {
                this.Exit();
            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void Playing(GameTime gameTime, KeyboardState keyboardState)
        {
            lastPause += gameTime.ElapsedGameTime.Milliseconds;

            //Check for pause
            if (keyboardState.IsKeyDown(Keys.Escape) && lastPause > PauseDelay)
            {
                gameState = GameState.Paused;
                lastPause = 0;
            }

            //Check if exit was reached
            if (level.ReachedExit && levelIndex < numLevels)
            {
                levelIndex++;
                level = new Level(Services, levelIndex);

            }
            else if (levelIndex >= numLevels && level.ReachedExit)
            {
                gameState = GameState.GameOver;              
            }

            //Check if player is out of lives
            if (level.Player.Lives <= 0)
                gameState = GameState.GameOver;

            level.Update(gameTime, Keyboard.GetState());
            hud.Update(gameTime, level);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void Paused(GameTime gameTime, MouseState mouseState, KeyboardState keyboardState)
        {
            lastPause += gameTime.ElapsedGameTime.Milliseconds;
            continuePlaying.Position = new Vector2(100, 340);
            quit.Position = new Vector2(100, 410);

            if (keyboardState.IsKeyDown(Keys.Escape) && lastPause > PauseDelay)
            {
                gameState = GameState.Playing;
                lastPause = 0;
            }

            if (continuePlaying.Update(gameTime, mouseState))
            {
                gameState = GameState.Playing;
            }

            if (quit.Update(gameTime, mouseState))
            {
                gameState = GameState.Start;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void GameOver(GameTime gameTime, MouseState mouseState)
        {
            playAgain.Position = new Vector2(100, 340);
            quit.Position = new Vector2(100, 410);

            if (playAgain.Update(gameTime, mouseState))
            {
                gameState = GameState.Playing;
                levelIndex = 1;
                level = new Level(Services, levelIndex);
            }

            if (quit.Update(gameTime, mouseState))
            {
                this.Exit();
            }

        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
           

            //Update the game based on which state the game is currently in
            switch (gameState)
            {
                case GameState.Start:
                    spriteBatch.Draw(startBackground, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    play.Draw(gameTime, spriteBatch);
                    instructions.Draw(gameTime, spriteBatch);
                    quit.Draw(gameTime, spriteBatch);
                    spriteBatch.Draw(cursorTexture, cursorPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    break;

                case GameState.Playing:
                    spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    drawLevel(gameTime, spriteBatch);
                    break;

                case GameState.Paused:
                    spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    drawLevel(gameTime, spriteBatch);
                    spriteBatch.Draw(pausedBackground, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    continuePlaying.Draw(gameTime, spriteBatch);
                    quit.Draw(gameTime, spriteBatch);
                    spriteBatch.Draw(cursorTexture, cursorPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    break;

                case GameState.GameOver:
                    spriteBatch.Draw(gameOverBackground, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    playAgain.Draw(gameTime, spriteBatch);
                    quit.Draw(gameTime, spriteBatch);
                    spriteBatch.Draw(cursorTexture, cursorPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    break;

                default:
                    throw new Exception("Invalid GameState.");
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void drawLevel(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.Instance.ViewMatrix);
            level.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            hud.Draw(gameTime, spriteBatch);
        }


    }


    //Used by the game class to determine game state
    enum GameState
    {
        //Waiting on the player to begin the game
        Start = 0,

        //Player currently playing game
        Playing = 1,

        //Player has paused the game
        Paused = 2,

        //Game over
        GameOver = 3,
    }


}
