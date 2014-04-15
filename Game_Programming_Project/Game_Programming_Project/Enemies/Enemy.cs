using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Programming_Project.Enemies
{


    class Enemy
    {

        //RPG variables
        public int Health
        {
            get { return health;  }
        }
        protected int health = 100;

        //Animations
        protected Animation idleAnimation;
        protected Animation walkAnimation;
        protected Animation attackAnimation;
        protected Animation jumpAnimation;
        protected Animation deathAnimation;
        protected SpriteEffects flip = SpriteEffects.None;
        protected Animator sprite;


        //Attack variables
        protected Vector2 attackPos;
        protected Vector2 attackVel = new Vector2(4, 0);
        protected int timeBetweenAttacks = 400;
        protected int lastAttack = 0;
        protected int attackDmg = 5;
        public List<Attack> Attacks
        {
            get { return attacks; }
        }
        protected List<Attack> attacks = new List<Attack>();


        //Enemy movement and position
        protected int attackRange = 300;
        protected Vector2 spawnLocation;
        protected Vector2 direction;
        protected float previousBottom;

        protected Vector2 MaxVelocity = new Vector2(0.5f, 0);

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        protected Vector2 position;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        protected Vector2 velocity;


        //Enemy state
        public bool IsStanding
        {
            get { return isStanding; }
        }
        protected bool isStanding;

        public bool Alerted
        {
            get { return alerted; }
        }
        protected bool alerted;

        protected bool hitWall = false;
        protected bool willFall = false;

        //Reference to the level the enemy is on
        public Level Level
        {
            get { return level; }
        }
        protected Level level;

        //Rectangle around the enemy
        protected Rectangle enemyBounds;
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
            spawnLocation = position;
            Spawn(spawnLocation);
        }


        /// <summary>
        /// Spawns the enemy at the given position
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Spawn(Vector2 position)
        {
            Position = position;
            direction.X = -1;
            Velocity = Vector2.Zero;
            sprite.PlayAnimation(idleAnimation);
        }


        /// <summary>
        /// Loads the enemy sprite sheet and sounds.
        /// </summary>
        public virtual void LoadContent()
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

            //direction = Vector2.Zero;
        }


        /// <summary>
        /// Handles the AI of the enemy
        /// </summary>
        protected virtual void EnemyAI(GameTime gameTime)
        {
            Vector2 playerPos = Level.Player.Position;
            lastAttack += gameTime.ElapsedGameTime.Milliseconds;
            int distance = (int)(playerPos.X - position.X);

            bool inRange = (Math.Abs(distance) < attackRange);

            bool facingPlayer = ((playerPos.X < position.X && direction.X < 0) ||
                (playerPos.X > position.X && direction.X > 0));

            bool inView = (playerPos.Y > (position.Y - 64) &&
                playerPos.Y < (position.Y + 128));

            //Check if the enemy can see the player or not
            if (inRange && facingPlayer && inView)                
            {
                alerted = true;
            }
            else if (alerted && (!inRange || !inView))
            {
                alerted = false;
            }


            if (alerted)
            {
                //Face the player
                if (playerPos.X > Position.X)
                    direction.X = 1;
                else if (playerPos.X < Position.X)
                    direction.X = -1;
                else
                    direction.X = 0;

                //Attack the player
                if (lastAttack >= timeBetweenAttacks)
                {
                    attackVel = new Vector2(10, 0);
                    Attack();
                    lastAttack = 0;
                }
            }

            //If enemy cannot see player, then just patrol the area
            else
            {
                if (hitWall || willFall)
                {
                    direction.X *= -1;
                    hitWall = false;
                    willFall = false;
                }

            }

           

        }


        /// <summary>
        /// Handles the movement of the enemy
        /// </summary>
        protected virtual void MoveEnemy(GameTime gameTime)
        {
            Vector2 previousPosition = Position;

            //Update velocity
            velocity.X = direction.X * MaxVelocity.X;
            velocity.Y = GamePhysics.GetFallSpeed(Velocity.Y, gameTime);

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
            attackPos.Y -= 38;
            attackVel *= direction;
            attacks.Add(new Attack(attackAnimation, attackPos, attackVel, attackDmg));
        }


        /// <summary>
        /// 
        /// </summary>
        public void hitByAttack(Attack attack)
        {
            this.health -= attack.Damage;
        }


        /// <summary>
        /// sdfs
        /// </summary>
        protected void HandleCollisions()
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
                                hitWall = true;

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
