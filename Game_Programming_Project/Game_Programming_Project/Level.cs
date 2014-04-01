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

        //Terrain structure of the level
        private Block[,] blocks; //2D array
        int BlockGridHeight = 18;
        int BlockGridWidth = 32;

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
            //Temporary map of chars which determine which block goes where.
            //Eventually, this needs to be read in from a file.
            char[,] blockMap = { 
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x','-', '-', '-', '-','x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x'},
                               {'x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x','-', '-', '-', '-','x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x'}
                               };


            blocks = new Block[BlockGridWidth, BlockGridHeight];

            //Creating each of the blocks in the grid system
            for (int y = 0; y < BlockGridHeight; y++)
            {
                for (int x = 0; x < BlockGridWidth; x++)
                {
                    //Load each block
                    char blockType = blockMap[y,x];
                    blocks[x, y] = LoadBlock(blockType, x, y);
                }
            }

        }


        //Determines which type of block is needed, and where.  Then, creates the block.
        private Block LoadBlock(char blockType, int x, int y)
        {

            switch (blockType)
            {
                //Air block
                case '-':
                    return new Block(null, BlockCollision.Passable);

                //Ground block
                case 'x':
                    return CreateBlock("block1", BlockCollision.Impassable);

                default:
                    throw new Exception("Error loading block");

            }


        }


        //Loads an individual block and returns it
        private Block CreateBlock(string name, BlockCollision collision)
        {
            return new Block(Content.Load<Texture2D>("Blocks/" + name), collision);
        }


        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {

            player.Update(gameTime, keyboardState);

        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw player
            Player.Draw(gameTime, spriteBatch);

            //Draw each of the blocks
            DrawBlocks(spriteBatch);

        }


        public void DrawBlocks(SpriteBatch spriteBatch)
        {

            for (int y = 0; y < BlockGridHeight; y++)
            {
                for (int x = 0; x < BlockGridWidth; x++)
                {
                    Texture2D blockTexture = blocks[x, y].Texture;
                    if (blockTexture != null)
                    {
                        // Draw it in screen space.
                        Vector2 position = new Vector2(x, y) * Block.Size;
                        spriteBatch.Draw(blockTexture, position, Color.White);
                    }

                }
            }

        }


    }
}