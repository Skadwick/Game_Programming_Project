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


    /// <summary>
    /// This class is loaded by the Game class, and contains the player object, the block grid system and all
    /// of the block objects, and enemy objects.  The content manager from the game class is passed here, so that
    /// content can be loaded.
    /// </summary>

    class Level
    {
        //Terrain structure of the level
        private Block[,] blocks; //2D array
        const int BlockGridHeight = 18;
        const int BlockGridWidth = 32;

        //Player object which the user controls within the level
        public Player Player
        {
            get { return player; }
        }
        Player player;

        //Starting position of the player
        private Vector2 start; 

        //Game content       
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;


        /// <summary>
        /// Constructor for the Level class.  serviceProvider is the Game.Services property from the
        /// Game class, which gets the the GameServiceContainer object holding all the service providers
        /// attached to the Game. The Level class uses this to create a copy of the Game's content manager.
        /// The constructor also begins loading the blocks needed for the level, and spawns the player at
        /// the proper location within the level.
        /// </summary>
        public Level(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");
            LoadBlocks();
            player = new Player(this, new Vector2(100, 100));
        }



        /// <summary>
        /// Begins creating Block objects based on a 2D char array which is hard-coded below (Should be read in from file).
        /// The symbols in the char array determine the type of block, and it's collision type.
        /// </summary>
        private void LoadBlocks()
        {
            //Creating a 2D array of characters which determine which block goes where
            char[,] blockMap = { 
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'x', 'x', 'x', 'x','x', 'x', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', 'x', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', 'x','x', 'x', 'x', 'x','x', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-','-', '-', '-', '-'},
                               {'-', '-', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x','-', '-', '-', '-','x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x'},
                               {'x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x','-', '-', '-', '-','x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x','x', 'x', 'x', 'x'}
                               };

            //Initialize the block grid
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



        /// <summary>
        /// Recieves a char which it uses in a switch statement to decide which type of
        /// block to create.  Once that determination is made, it creates the block and
        /// returns it.
        /// </summary>
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

                //Default should only be reached if an incorrect char symbol was entered
                default:
                    throw new Exception("Error loading block");
            }
        }
        


        /// <summary>
        /// Creates a new block with the proper collision type, and loads the texture based 
        /// on the string argument.
        /// </summary>
        private Block CreateBlock(string name, BlockCollision collision)
        {
            return new Block(Content.Load<Texture2D>("Blocks/" + name), collision);
        }



        /// <summary>
        /// Returns the collision type of a block within the grid system
        /// </summary>
        public BlockCollision GetCollision(int x, int y)
        {
            // Prevent escaping past the level ends.
            if (x < 0 || x >= BlockGridWidth)
                return BlockCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= BlockGridHeight)
                return BlockCollision.Passable;

            return blocks[x, y].Collision;
        }



        /// <summary>
        /// dfhgdf
        /// </summary>
        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {

            player.Update(gameTime, keyboardState);

        }



        /// <summary>
        /// asdfa
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw player
            Player.Draw(gameTime, spriteBatch);

            //Draw each of the blocks
            DrawBlocks(spriteBatch);

        }



        /// <summary>
        /// adsfa
        /// </summary>
        public void DrawBlocks(SpriteBatch spriteBatch)
        {
            //Looping through each of the blocks in the grid
            for (int y = 0; y < BlockGridHeight; y++)
            {
                for (int x = 0; x < BlockGridWidth; x++)
                {
                    Texture2D blockTexture = blocks[x, y].Texture;
                    if (blockTexture != null)
                    {
                        //Find the proper location of the block and draw it
                        Vector2 position = new Vector2(x, y) * Block.Size;
                        spriteBatch.Draw(blockTexture, position, Color.White);
                    }
                }
            }
        }


    }
}