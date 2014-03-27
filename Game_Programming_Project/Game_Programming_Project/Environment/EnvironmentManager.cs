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

            testBlock = new Block(Game.Content.Load<Texture2D>(@"Images/Environment/block1"),
                new Vector2(400,500), 5, Vector2.Zero);

            base.LoadContent();
        }


        /*
         * 
         */
        public override void Update(GameTime gameTime)
        {
            testBlock.Update(gameTime, Game.Window.ClientBounds);

            //Check if the player is touching any of the blocks.
            if (SpriteManager.player.collisionRect.Intersects(testBlock.collisionRect))
            {
                Vector2 collisionSide = new Vector2(0,0); //Will tell the SpriteManager where the collision occured.

                //Hit left side of terrain
                if( SpriteManager.player.pos.X >= (testBlock.pos.X - SpriteManager.player.frameSize.X) &&
                    SpriteManager.player.direction.X > 0)
                {
                    collisionSide.X = -1;
                }

                //Hit right side of terrain
                if (SpriteManager.player.pos.X <= (testBlock.pos.X + testBlock.frameSize.X) &&
                    SpriteManager.player.direction.X < 0)
                {
                    collisionSide.X = 1;
                }

                //Hit top of terrain
                if (SpriteManager.player.pos.Y >= (testBlock.pos.Y - SpriteManager.player.frameSize.Y))
                {
                    collisionSide.Y = 1;
                }



                SpriteManager.player.collisionLocation = collisionSide;
            }

            base.Update(gameTime);
        }


        /*
         * 
         */
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            testBlock.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
