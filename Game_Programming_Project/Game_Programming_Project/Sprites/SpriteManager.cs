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

        //SpriteBatch object used for drawing the sprites
        SpriteBatch spriteBatch;

        //Player variables
        PlayerSprite player;
        Vector2 playerSpeed = new Vector2(4, 4);


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

            player = new PlayerSprite(
                Game.Content.Load<Texture2D>(@"Images/Player/idle"),
                new Vector2(200,200), new Point(65,60), 5, new Point(0,0),
                new Point(2, 1), playerSpeed, 256);

            base.LoadContent();
        }


        /*
         * 
         */
        public override void Update(GameTime gameTime)
        {

            //Player is jumping
            if (player.jumping)
            {
                player.setAnimation(Game.Content.Load<Texture2D>(@"Images/Player/jumping"), 
                    new Point(50, 69), new Point(0, 0), new Point(1, 1), 1000);
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
                    new Point(65, 60), new Point(0, 0), new Point(2, 1), 256);
            }

            //Player is running right
            else if (player.direction.X > 1 && player.direction.X != 0 && !player.attacking &&
                !player.texture.Equals(Game.Content.Load<Texture2D>(@"Images/Player/runningRight")))
            {
                player.setAnimation(Game.Content.Load<Texture2D>(@"Images/Player/runningRight"),
                    new Point(45, 60), new Point(0, 0), new Point(6, 1), 75);
            }

            //Player is running left
            else if (player.direction.X < 1 && player.direction.X != 0 && !player.attacking &&
                !player.texture.Equals(Game.Content.Load<Texture2D>(@"Images/Player/runningLeft")))
            {
                player.setAnimation(Game.Content.Load<Texture2D>(@"Images/Player/runningLeft"),
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

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
