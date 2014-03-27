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
    class EnvironmentManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        SpriteBatch spriteBatch;

        List<Sprite> terrainBlocks = new List<Sprite>();

        Block testBlock;

        /*
         * Constructor
         */
        public EnvironmentManager(Game game)
            : base(game)
        {
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

            /*
            testBlock = new Block(Game.Content.Load<Texture2D>(@"Images/Environment/block1"),
                new Vector2(400,510), 0, Vector2.Zero);
            terrainBlocks.Add(testBlock);
             */

            //Testing terrain generation within a loop
            int ex = 900;
            int wy = 495;
            for(int a = 1; a <= 12; a++)
            {
                for (int b = a; b < 10; b++)
                {
                    ex -= 50;
                    terrainBlocks.Add( new Block(Game.Content.Load<Texture2D>(@"Images/Environment/block1"),
                        new Vector2(ex, wy), 0, Vector2.Zero));
                }
                ex = 900;
                wy -= 50;
            }


            base.LoadContent();
        }


        /*
         * 
         */
        public override void Update(GameTime gameTime)
        {
            

            //Handling player collision with terrain blocks
            Vector2 collisionSide = new Vector2(0, 0); //Will tell the SpriteManager where the collision occured.
            foreach(Sprite tb in terrainBlocks)
            {
                tb.Update(gameTime, Game.Window.ClientBounds);

                if (SpriteManager.player.collisionRect.Intersects(tb.collisionRect))
                {
                    
                    Vector2 pPosition = SpriteManager.player.pos;

                    //Hit top of terrain
                    if ((SpriteManager.player.pos.Y >= (tb.pos.Y - SpriteManager.player.frameSize.Y)) &&
                        (SpriteManager.player.pos.Y <= (tb.pos.Y - SpriteManager.player.frameSize.Y + SpriteManager.FALLSPEED)))
                    {
                        collisionSide.Y = 1;
                    }

                    //Hit left side of terrain
                    else if (SpriteManager.player.pos.X >= (tb.pos.X - SpriteManager.player.frameSize.X) &&
                        SpriteManager.player.direction.X > 0)
                    {
                        collisionSide.X = -1;
                    }

                    //Hit right side of terrain
                    else if (SpriteManager.player.pos.X <= (tb.pos.X + tb.frameSize.X) &&
                        SpriteManager.player.direction.X < 0)
                    {
                        collisionSide.X = 1;
                    }
                }

            }
            SpriteManager.player.collisionLocation = collisionSide;
            

            base.Update(gameTime);
        }


        /*
         * 
         */
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            foreach (Sprite tb in terrainBlocks)
            {
                tb.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
