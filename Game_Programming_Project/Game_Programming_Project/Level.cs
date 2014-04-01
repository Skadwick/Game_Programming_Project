using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace Game_Programming_Project
{
    class Level
    {

        //Player entities in the level.
        public Player Player
        {
            get { return player; }
        }
        Player player;

        private Vector2 start;

        // Level content       
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;


        public Level(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");
            LoadBlocks();
            player = new Player(this, new Vector2(100, 100));
        }


        //Begins loading the layout of the various blocks of the level
        private void LoadBlocks()
        {

        }


        //Determines which type of block is needed, and where.  Then, creates the block.
        private void LoadBlock()
        {

        }


        //Loads an individual block and returns it
        private Block CreateBlock(string name, BlockCollision collision)
        {
            return new Block(Content.Load<Texture2D>("Tiles/" + name), collision);
        }


        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {

            player.Update(gameTime, keyboardState);

        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Player.Draw(gameTime, spriteBatch);
        }


    }
}