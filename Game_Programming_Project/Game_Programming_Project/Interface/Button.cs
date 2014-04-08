using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Programming_Project.Interface
{
    class Button
    {
        //Button Textures
        private Texture2D texture;
        private Texture2D buttonTexture;
        private Texture2D hoverTexture;

        //Button position
        private Vector2 position;

        //Rectangle around button
        Rectangle buttonBounds;

        //Button constructor
        public Button(Texture2D btnTxt, Vector2 btnPos)
        {
            buttonTexture = btnTxt;
            hoverTexture = btnTxt;
            texture = buttonTexture;
            position = btnPos;

            buttonBounds = new Rectangle((int)position.X, (int)position.Y, 
                texture.Width, texture.Height);
        }


        public Button(Texture2D btnTxt, Texture2D hvrTxt, Vector2 btnPos)
        {
            buttonTexture = btnTxt;
            hoverTexture = hvrTxt;
            texture = buttonTexture;
            position = btnPos;

            buttonBounds = new Rectangle((int)position.X, (int)position.Y,
                texture.Width, texture.Height);
        }


        //Update the button
        public bool Update(GameTime gameTime, MouseState mouseState)
        {
            Point mousePos = new Point(mouseState.X, mouseState.Y);

            if (buttonBounds.Contains(mousePos))
            {
                texture = hoverTexture;
                if (mouseState.LeftButton == ButtonState.Pressed)
                    return true;
            }
            else
            {
                texture = buttonTexture;
            }

            return false;
        }


        //Draw the button
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

    }
}
