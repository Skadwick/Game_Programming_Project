using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Game_Programming_Project.Enemies;

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
        private char[,] blockMap = new char[BlockGridWidth, BlockGridHeight];

        //Specific locations within the level     
        private Vector2 start;
        private Point exit = new Point(-1, -1);

        //Player object which the user controls within the level
        public Player Player
        {
            get { return player; }
        }
        Player player;

        //Other sprite objects within the level
        List<Enemy> enemies = new List<Enemy>();

        //Game content       
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        public int LevelIndex
        {
            get { return levelIndex; }
            set { levelIndex = value; }
        }
        private int levelIndex;

        public bool ReachedExit
        {
            get { return reachedExit; }
            set { reachedExit = value; }
        }
        private bool reachedExit;


        /// <summary>
        /// Constructor for the Level class.  serviceProvider is the Game.Services property from the
        /// Game class, which gets the the GameServiceContainer object holding all the service providers
        /// attached to the Game. The Level class uses this to create a copy of the Game's content manager.
        /// The constructor also begins loading the blocks needed for the level, and spawns the player at
        /// the proper location within the level.
        /// </summary>
        public Level(IServiceProvider serviceProvider, int lvlIndex)
        {
            content = new ContentManager(serviceProvider, "Content");
            LevelIndex = lvlIndex;
            LoadLevel();
            LoadBlocks();
            //player = new Player(this, new Vector2(100, 100));
        }


        /// <summary>
        /// 
        /// </summary>
        private void LoadLevel()
        {
            //Get the level layout from the file
            string filePath = string.Format("Content/Levels/level{0}.txt", levelIndex);
            Stream fileStream = TitleContainer.OpenStream(filePath);

            using (StreamReader reader = new StreamReader(fileStream))
            {
                int lineIndex = 0;
                string line = reader.ReadLine();
                while (line != null)
                {
                    char[] charLine = line.ToCharArray();
                    for (int i = 0; i < charLine.Length; i++)
                        blockMap[i, lineIndex] = charLine[i];

                    lineIndex++;
                    line = reader.ReadLine();
                }
            }
        }


        /// <summary>
        /// Begins creating Block objects based on a 2D char array which is hard-coded below (Should be read in from file).
        /// The symbols in the char array determine the type of block, and it's collision type.
        /// </summary>
        private void LoadBlocks()
        {
            //Initialize the block grid
            blocks = new Block[BlockGridWidth, BlockGridHeight];

            //Creating each of the blocks in the grid system
            for (int y = 0; y < BlockGridHeight; y++)
            {
                for (int x = 0; x < BlockGridWidth; x++)
                {
                    //Load each block
                    char blockType = blockMap[x,y];
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
                case '#':
                    //return CreateBlock("block1", BlockCollision.Impassable); //Enemies currently cannot walk left across Impassible blocks (???)
                    return CreateBlock("block1", BlockCollision.Impassable);

                //Platform block
                case 'p':
                    return CreateBlock("block2", BlockCollision.Platform);

                //Player spawn
                case 's':
                    return SpawnPlayer(x, y);

                //Level exit block
                case 'e':
                    return LevelExit(x, y);

                //Enemy spawn
                case '@':
                    return SpawnEnemy(blockType, x, y);
                case '%':
                    return SpawnEnemy(blockType, x, y);

                //Default should only be reached if an incorrect char symbol was entered
                default:
                    return new Block(null, BlockCollision.Passable);
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
        /// Spawns an enemy at the given location, and creates an air block behind it
        /// </summary>
        private Block SpawnEnemy(char type, int x, int y)
        {
            Vector2 pos = new Vector2(x * Block.Size.X, ((y+1) * Block.Size.Y));

            if (type == '@')
                enemies.Add(new Enemy(this, pos));
            else if (type == '%')
                enemies.Add(new Mech2(this, pos));

            return new Block(null, BlockCollision.Passable);
        }


        /// <summary>
        /// Spawns the player at the given location
        /// </summary>
        private Block SpawnPlayer(int x, int y)
        {
            start = new Vector2(x * Block.Size.X, ((y + 1) * Block.Size.Y));
            player = new Player(this, start);

            return new Block(null, BlockCollision.Passable);
        }


        /// <summary>
        /// Spawns the player at the given location
        /// </summary>
        private Block LevelExit(int x, int y)
        {
            exit = GetBounds(x, y).Center;
            return CreateBlock("exit", BlockCollision.Passable);
        }



        /// <summary>
        /// Gets the bounding rectangle of a tile in world space
        /// </summary>        
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * Block.Width, y * Block.Height, Block.Width, Block.Height);
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

            //Return collision type of block at [x,y]
            return blocks[x, y].Collision;
        }



        /// <summary>
        /// dfhgdf
        /// </summary>
        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            List<Enemy> removeEnemies = new List<Enemy>(); //Enemies to be removed

            //Update the player
            player.Update(gameTime, keyboardState);

            //Check if player reached the exit
            if (Player.PlayerRect.Contains(exit))
            {
                ExitReached();
            }

            //Update each of the enemies
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);

                List<Attack> removeAttacks = new List<Attack>();

                foreach (Attack enemyAttack in enemy.Attacks)
                {
                    //Check if the attack hit the player
                    if (player.PlayerRect.Intersects(enemyAttack.EnemyRect))
                    {
                        player.hitByAttack(enemyAttack);
                        removeAttacks.Add(enemyAttack);
                    }

                    //Check if the attack is off the screen
                    if (enemyAttack.Position.X < 0 || enemyAttack.Position.X > Game.resolution.X ||
                        enemyAttack.Position.Y < 0 || enemyAttack.Position.Y > Game.resolution.Y)
                    {
                        removeAttacks.Add(enemyAttack);
                    }
                }

                //Delete the necessary attacks
                foreach (Attack remove in removeAttacks)
                    enemy.Attacks.Remove(remove);
            }


            //When the player dies, do this.
            if (player.Health <= 0)
                player.Reset(start);
        }


        /// <summary>
        /// Is called once the player reaches the end of the level
        /// </summary>
        private void ExitReached()
        {
            reachedExit = true;
        }



        /// <summary>
        /// asdfa
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw each of the blocks
            DrawBlocks(spriteBatch);

            //Draw each of the enemies
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

            //Draw player
            Player.Draw(gameTime, spriteBatch);

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