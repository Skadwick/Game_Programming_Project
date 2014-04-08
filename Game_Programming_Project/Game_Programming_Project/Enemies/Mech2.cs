using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Programming_Project.Enemies
{
    class Mech2: Enemy
    {



        /// <summary>
        /// Constructs a new Mech2
        /// </summary>
        public Mech2(Level level, Vector2 position) 
            : base(level, position)
        {

            attackVel = new Vector2(5, 0);
            timeBetweenAttacks = 800;
            attackDmg = 15;
            MaxVelocity = new Vector2(0.3f, 0);

            //LoadContent();

        }


        /// <summary>
        /// Loads the enemy sprite sheet and sounds.
        /// </summary>
        public override void LoadContent()
        {
            idleAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Enemy/Mech2/Mech2Idle"), 0.1f, true);
            walkAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Enemy/Mech2/Mech2Walk"), 0.15f, true);
            deathAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Enemy/Mech2/Mech2Death"), 0.1f, false);

            //Create a rectangle used to represent the player's bounds           
            int width = (int)(idleAnimation.FrameWidth * 0.6);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameWidth * 0.9);
            int top = idleAnimation.FrameHeight - height;
            enemyBounds = new Rectangle(left, top, width, height);
        }


        /// <summary>
        /// Handles the AI of the enemy
        /// </summary>
        protected virtual void EnemyAI(GameTime gameTime)
        {
            Vector2 playerPos = Level.Player.Position;
            lastAttack += gameTime.ElapsedGameTime.Milliseconds;

            //Face the player
            if (playerPos.X > Position.X)
                direction.X = 1;
            else if (playerPos.X < Position.X)
                direction.X = -1;
            else
                direction.X = 0;
            /*
            if (lastAttack >= timeBetweenAttacks)
            {
                attackVel = new Vector2(10, 0);
                Attack();
                lastAttack = 0;
            }
            */
        }


        /// <summary>
        /// Handles the movement of the enemy
        /// </summary>
        protected virtual void MoveEnemy(GameTime gameTime)
        {
            Vector2 previousPosition = Position;

            //Update velocity
            velocity.X = direction.X * MaxVelocity.X;
            velocity.Y = 4;

            //Apply velocity to enemy
            position += Velocity;

            HandleCollisions();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (Position.X == previousPosition.X)
                velocity.X = 0;
            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }


        protected virtual void Attack()
        {
            //Create the animation to be used by the attack
            Animation attackAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Enemy/shot"), 0.1f, true);

            //Calculate position and velocity
            attackPos = Position;
            attackPos.Y -= 45;
            attackVel *= direction;
            attacks.Add(new Attack(attackAnimation, attackPos, attackVel, attackDmg));
        }






    }
}
