using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Programming_Project
{
    class Player
    {

        //Player RPG variables
        private const int MaxHealth = 100;
        private const int DefaultLives = 3;

        public int Health
        {
            get { return health; }
        }
        private int health;

        public int Lives
        {
            get { return lives; }
        }
        private int lives;

        public bool IsAttacking
        {
            get { return isAttacking; }
        }
        private bool isAttacking;


        //Animations
        private Animation idleAnimation;
        private Animation runAnimation;
        private Animation jumpAnimation;
        private Animation attackAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private Animator sprite;

        //Player movement and position
        private Vector2 spawnPoint;
        private Vector2 direction;
        private float previousBottom;
        private float previousTop;

        private Vector2 MaxVelocity = new Vector2(5, 5);
        private float jumpVel = -7.5f;

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

        public bool IsJumping
        {
            get { return isJumping; }
        }
        bool isJumping;

        public bool WasJumping
        {
            get { return wasJumping; }
        }
        bool wasJumping;

        //Reference to the level the player is on
        public Level Level
        {
            get { return level; }
        }
        Level level;

        //Rectangle around the player
        private Rectangle playerBounds;
        public Rectangle PlayerRect
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + playerBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + playerBounds.Y;

                return new Rectangle(left, top, playerBounds.Width, playerBounds.Height);
            }
        }
             

        /// <summary>
        /// Constructs a new player
        /// </summary>
        public Player(Level level, Vector2 position)
        {
            this.level = level;
            LoadContent();
            spawnPoint = position;
            Reset(spawnPoint);
            lives = DefaultLives;
        }


        /// <summary>
        /// Returns the player to life.
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Reset(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
            sprite.PlayAnimation(idleAnimation);
            health = MaxHealth;
        }


        /// <summary>
        /// Loads the player sprite sheet and sounds.
        /// </summary>
        public void LoadContent()
        {
            idleAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/idle"), 0.1f, true);
            attackAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/attack"), 0.1f, true);
            runAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/running"), 0.12f, true);
            jumpAnimation = new Animation(Level.Content.Load<Texture2D>("Sprites/Player/jumping"), 0.1f, true);

            //Create a rectangle used to represent the player's bounds           
            int width = (int)(idleAnimation.FrameWidth * 0.4);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameWidth * 0.8);
            int top = idleAnimation.FrameHeight - height;
            playerBounds = new Rectangle(left, top, width, height);
        }


        /// <summary>
        /// Handles input, performs physics, and animates the player sprite.
        /// </summary>
        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            GetInput(keyboardState);
            MovePlayer(gameTime);

            //Play the proper animation
            if (IsAttacking)
                sprite.PlayAnimation(attackAnimation);
            else if (isJumping || wasJumping)
                sprite.PlayAnimation(jumpAnimation);
            else if (Velocity.X != 0)
                sprite.PlayAnimation(runAnimation);
            else
                sprite.PlayAnimation(idleAnimation);

            //Check the player's health
            if (Health <= 0)
                killed();
            
            direction = Vector2.Zero;

            //Sets the focal point for the camera by getting the coordinates of the player relative to the screen
            //Moves with the player in both the x and y directions
            Camera.Instance.SetFocalPoint(new Vector2(position.X, position.Y));
        }


        /// <summary>
        /// Gets player movement and jumpinput
        /// </summary>
        private void GetInput(KeyboardState keyboardState)
        {
            //Check for input for horizontal movement
            if ((Keyboard.GetState().IsKeyDown(Keys.Left) ||
                Keyboard.GetState().IsKeyDown(Keys.A)))
            {
                direction.X -= 1;
            }
            else if ((Keyboard.GetState().IsKeyDown(Keys.Right) ||
                Keyboard.GetState().IsKeyDown(Keys.D)))
            {
                direction.X += 1;
            }

            //Check  if player is attacking
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }

            isJumping = (keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up) || 
                keyboardState.IsKeyDown(Keys.W) ) && !wasJumping;
        }


        /// <summary>
        /// Updates the players position as needed
        /// </summary>
        public void MovePlayer(GameTime gameTime)
        {
            Vector2 previousPosition = Position;

            //Update velocity
            velocity.X = direction.X * MaxVelocity.X;
            velocity.Y = GamePhysics.GetFallSpeed(Velocity.Y, gameTime);

            if (isStanding)
            {
                velocity.Y = 0;
            }

            //Check for jumping
            velocity.Y = Jump(velocity.Y, gameTime);

            //Apply velocity to player position
            position += Velocity;

            HandleCollisions();

            //Check if the player fell off the map
            if (Position.Y >= Game.resolution.Y)
                killed();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (Position.X == previousPosition.X)
                velocity.X = 0;
            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }



        /// <summary>
        /// sdfs
        /// </summary>
        private float Jump(float yVel, GameTime gameTime)
        {
            if (IsJumping && !wasJumping)
            {
                wasJumping = true;
                yVel = jumpVel;
            }

            if (wasJumping)
                sprite.PlayAnimation(jumpAnimation);

            return yVel;
        }



        /// <summary>
        /// sdfs
        /// </summary>
        private void HandleCollisions()
        {
            Rectangle bounds = PlayerRect;

            isStanding = false;

            //Finding neighboring blocks
            int leftBlock = (int)Math.Floor( (float)bounds.Left / Block.Width );
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
                                    wasJumping = false;
                                }

                                //Check if player is colliding with bottom of an impassable block
                                if (previousTop >= blockRect.Bottom && blockCollision == BlockCollision.Impassable)
                                {
                                    velocity.Y = 0;
                                }

                                // Ignore platforms, unless we are on the ground
                                if (blockCollision == BlockCollision.Impassable || IsStanding)
                                {
                                    //resolve the collision along the Y axis
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);
                                    
                                    //Update bounds
                                    bounds = PlayerRect;
                                }
                            }

                            else if (blockCollision == BlockCollision.Impassable) // Ignore platforms
                            {
                                // Resolve the collision along the X axis.
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                //Update bounds
                                bounds = PlayerRect;
                            }
                        }
                    }
                }
            }

            // Save the new bounds bottom.
            previousBottom = bounds.Bottom;
            previousTop = bounds.Top;
        }


        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public void hitByAttack(Attack attack)
        {
            this.health -= attack.Damage;
        }


        public void killed()
        {
            lives--;
            this.Reset(spawnPoint);
        }


        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way we are moving.
            if (Velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (Velocity.X < 0)
                flip = SpriteEffects.None;

            // Draw that sprite.
            sprite.Draw(gameTime, spriteBatch, Position, flip);
        }        


    }
}
