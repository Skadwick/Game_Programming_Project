using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Programming_Project
{
    class Enemy
    {

        //stuff
        private int timeBetweenAttacks = 1200;
        private int lastAttack = 0;
        public List<Attack> attacks = new List<Attack>();
        private Vector2 attackPos;
        private Vector2 attackVel = new Vector2(10,0);
        private int attackDmg = 10;

        //Animations
        private Animation idleAnimation;
        private Animation walkAnimation;
        private Animation deathAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private Animator sprite;

        //Enemy movement and position
        private Vector2 direction;
        private float previousBottom;

        private Vector2 MaxVelocity = new Vector2(0.5f, 0);

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


        //Player state
        public bool IsStanding
        {
            get { return isStanding; }
        }
        bool isStanding;

        //Reference to the level the enemy is on
        public Level Level
        {
            get { return level; }
        }
        Level level;

        //Rectangle around the player
        private Rectangle enemyBounds;
        public Rectangle EnemyRect
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + enemyBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + enemyBounds.Y;

                return new Rectangle(left, top, enemyBounds.Width, enemyBounds.Height);
            }
        }
             

        /// <summary>
        /// Constructs a new enemy
        /// </summary>
        public Enemy(Level level, Vector2 position)
        {
            this.level = level;
            LoadContent();
            Spawn(position);
        }


        /// <summary>
        /// Spawns the enemy at the given position
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Spawn(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            sprite.PlayAnimation(idleAnimation);
        }


        /// <summary>
        /// Loads the enemy sprite sheet and sounds.
        /// </summary>
        public void LoadContent()
        {
            idleAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Enemy/idle"), 0.1f, true);
            walkAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Enemy/walk"), 0.15f, true);
            deathAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Enemy/death"), 0.1f, false);

            //Create a rectangle used to represent the player's bounds           
            int width = (int)(idleAnimation.FrameWidth * 0.6);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameWidth * 0.9);
            int top = idleAnimation.FrameHeight - height;
            enemyBounds = new Rectangle(left, top, width, height);
        }


        /// <summary>
        /// Updates various aspects of the enemy object
        /// </summary>
        public void Update(GameTime gameTime)
        {

            EnemyAI(gameTime);
            MoveEnemy(gameTime);

            foreach (Attack attack in attacks)
                attack.Update(gameTime);

            //Update animations
            if (Velocity.X != 0)
                sprite.PlayAnimation(walkAnimation);
            else
                sprite.PlayAnimation(idleAnimation);

            direction = Vector2.Zero;
        }


        /// <summary>
        /// Handles the AI of the enemy
        /// </summary>
        private void EnemyAI(GameTime gameTime)
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

            if (lastAttack >= timeBetweenAttacks)
            {
                attackVel = new Vector2(10, 0);
                Attack();
                lastAttack = 0;
            }

        }


        /// <summary>
        /// Handles the movement of the enemy
        /// </summary>
        private void MoveEnemy(GameTime gameTime)
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


        private void Attack()
        {
            //Create the animation to be used by the attack
            Animation attackAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Enemy/shot"), 0.1f, true);

            //Calculate position and velocity
            attackPos = Position;
            attackPos.Y -= 45;
            attackVel *= direction;
            attacks.Add(new Attack(attackAnimation, attackPos, attackVel, attackDmg));
        }


        /// <summary>
        /// sdfs
        /// </summary>
        private void HandleCollisions()
        {
            Rectangle bounds = EnemyRect;

            isStanding = false;

            //Finding neighboring blocks
            int leftBlock = (int)Math.Floor((float)bounds.Left / Block.Width);
            int rightBlock = (int)Math.Ceiling(((float)bounds.Right / Block.Width)) - 1;
            int topBlock = (int)Math.Floor((float)bounds.Top / Block.Height);
            int bottomBlock = (int)Math.Ceiling(((float)bounds.Bottom / Block.Height)) - 1;

            //Loop through each of the possible block collisions
            for (int y = topBlock; y <= bottomBlock; y++)
            {
                for (int x = leftBlock; x <= rightBlock; x++)
                {
                    //Checking collision type of block at the given coordinates
                    BlockCollision blockCollision = Level.GetCollision(x, y);
                    if (blockCollision != BlockCollision.Passable)
                    {
                        Rectangle blockRect = new Rectangle(x * Block.Width, y * Block.Height,
                            Block.Width, Block.Height);

                        //Check for collision
                        Vector2 depth = GameMath.CollisionDepth(bounds, blockRect);
                        if (depth != Vector2.Zero)
                        {

                            if (Math.Abs(depth.Y) <= Math.Abs(depth.X) || blockCollision == BlockCollision.Platform)
                            {
                                //Check if player is on the ground
                                if (previousBottom <= blockRect.Top)
                                {
                                    isStanding = true;
                                }

                                // Ignore platforms, unless we are on the ground
                                if (blockCollision == BlockCollision.Impassable || IsStanding)
                                {
                                    //resolve the collision along the Y axis
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);
                                    velocity.Y = 0;

                                    //Update bounds
                                    bounds = EnemyRect;
                                }
                            }

                            else if (blockCollision == BlockCollision.Impassable) // Ignore platforms
                            {
                                // Resolve the collision along the X axis.
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                //Update bounds
                                bounds = EnemyRect;
                            }
                        }
                    }
                }
            }

            // Save the new bounds bottom.
            previousBottom = bounds.Bottom;
        }


        /// <summary>
        /// Draws the animated enemy
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way it is moving
            if (Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (Velocity.X < 0)
                flip = SpriteEffects.None;

            foreach (Attack attack in attacks)
                attack.Draw(gameTime, spriteBatch);

            // Draw that sprite.
            sprite.Draw(gameTime, spriteBatch, Position, flip);
        }


    }
}
