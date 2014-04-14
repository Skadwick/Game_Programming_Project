using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Programming_Project.Interface
{
    class PlayerHud
    {
        //Values to display
        int playerHealth;
        int playerLives;
        int playTime;

        //Variables to draw the HUD
        SpriteFont healthTxt, livesTxt, timeTxt;

        //Game content       
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        public PlayerHud(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");
            LoadContent();
        }


        public void Reset()
        {
            playTime = 0;
        }


        private void LoadContent()
        {
            healthTxt = Content.Load<SpriteFont>(@"Fonts/Generic");
            livesTxt = Content.Load<SpriteFont>(@"Fonts/Generic");
            timeTxt = Content.Load<SpriteFont>(@"Fonts/Generic");
        }


        public void Update(GameTime gameTime, Level level)
        {
            playTime = level.PlayTime;
            playerHealth = level.Player.Health;
            playerLives = level.Player.Lives;
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(healthTxt, (playerHealth.ToString("N0") + "hp"), new Vector2(30, 10), Color.White);
            spriteBatch.DrawString(livesTxt, ("Lives: " + playerLives.ToString("N0")), new Vector2(30, 34), Color.White);
            spriteBatch.DrawString(timeTxt, ("Time: " + playTime.ToString("N0")), new Vector2(30, 58), Color.White);

        }


    }
}
