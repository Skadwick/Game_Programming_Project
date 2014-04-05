using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Programming_Project
{


    class Attack
    {

        private Animation attackAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private Animator sprite;

        public int Damage
        {
            get { return damage;  }
        }
        int damage;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;


        //Rectangle for the attack hitbox
        private Rectangle attackBounds;
        public Rectangle EnemyRect
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + attackBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + attackBounds.Y;

                return new Rectangle(left, top, attackBounds.Width, attackBounds.Height);
            }
        }


        public Attack(Animation animation, Vector2 pos, Vector2 vel, int dmg)
        {
            this.attackAnimation = animation;
            this.position = pos;
            this.velocity = vel;
            this.damage = dmg;
            sprite.PlayAnimation(attackAnimation);

            //Create a rectangle used to represent the player's bounds           
            int width = (int)attackAnimation.FrameWidth;
            int left = attackAnimation.FrameWidth - width;
            int height = (int)attackAnimation.FrameWidth;
            int top = attackAnimation.FrameHeight - height;
            attackBounds = new Rectangle(left, top, width, height);
        }


        /// <summary>
        /// Updates various aspects of the attack object
        /// </summary>
        public void Update(GameTime gameTime)
        {
            position += velocity;
        }



        /// <summary>
        /// Draws the animated attack
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way it is moving
            if (Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (Velocity.X < 0)
                flip = SpriteEffects.None;

            // Draw that sprite.
            sprite.Draw(gameTime, spriteBatch, Position, flip);
        }

    }
}
