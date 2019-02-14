using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakernoidsGL
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bgTexture;


        int lives = 0;


        Random random = new Random();
        double spawnChance = 1;
        bool destroyPowerUp = false;


        bool ballCatch = false;
        


        Paddle paddle;
        List<Ball> balls = new List<Ball>();
        SoundEffect ballBounceSFX, ballHitSFX, deathSFX, powerUpSFX;
        List<PowerUp> powerups = new List<PowerUp>();
        


        List<Block> blocks = new List<Block>();
        int[,] blockLayout = new int[,]{
            {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
            {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
            {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
        };
        Block blockToDestroy;


        int colTimer = 20;
        





        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
        }





        protected void SpawnPowerUp (Vector2 postion)
        {            
            int type = random.Next(3);
            PowerUp temppower = new PowerUp((PowerUp.PowerUpType)type, this);
            temppower.position = blockToDestroy.position;
            temppower.LoadContent();
            powerups.Add(temppower);
        }

        //check for power ups
        private void CheckForPowerUps (PowerUp powerup)
        {
            if (paddle.BoundingRect.Intersects(powerup.BoundingRect))
            {
                ActivatePowerUp(powerup);
            }

        }

        private void ActivatePowerUp(PowerUp powerup)
        {
            for (int i = powerups.Count - 1; i >= 0; i--)
            {
                if (powerups[i] == powerup) 
                {
                    powerups[i].readyToDestroy = true;
                    destroyPowerUp = true;
                }
            }
            powerUpSFX.Play();

            switch (powerup.powerUpType)
            {
                case (PowerUp.PowerUpType)0:
                    ballCatch = true;
                    break;
                case (PowerUp.PowerUpType)1:
                    SpawnBall();
                    break;
                case (PowerUp.PowerUpType)2:
                    paddle.longPaddle = true;
                    paddle.SwitchPaddle();
                    break;
            }

        }

        private void SpawnBall()
        {
            Ball newBall = new Ball(this);
            balls.Add(newBall);

            newBall.LoadContent();
            newBall.position = new Vector2(paddle.position.X, paddle.position.Y - paddle.Height - newBall.Height);
        }


        private void DestroyBalls()
        {
            for (int i = balls.Count - 1; i >= 0; i--)
            {
                if (balls[i].destroy)
                {
                    Ball tempBall = balls[i];
                    balls.Remove(tempBall);
                }
            }
        }

        protected void LoseLife()
        {
            paddle.position = new Vector2(512, 740);
            SpawnBall();
            ballCatch = false;
            paddle.longPaddle = false;
            paddle.SwitchPaddle();
            deathSFX.Play();
        }

        protected void CheckCollisions(Ball ball)
        {
            float radius = ball.Width / 2;

            //paddle collision
            if (ball.colTimer == 0)
            {
                //center collision
                if ((ball.position.X > (paddle.position.X - paddle.Width / 6)) &&
                     (ball.position.X < (paddle.position.X + paddle.Width / 6)) &&
                     (ball.position.Y < paddle.position.Y) &&
                     (ball.position.Y > (paddle.position.Y - radius - paddle.Height / 2)))
                {
                    ball.direction = Vector2.Reflect(ball.direction, new Vector2(0, -1));
                    ball.colTimer = 60;
                    ballBounceSFX.Play();
                    Console.WriteLine("Center");
                }
                //right colision
                if ((ball.position.X > (paddle.position.X + paddle.Width / 6)) &&
                     (ball.position.X < (paddle.position.X + radius + paddle.Width / 2)) &&
                     (ball.position.Y < paddle.position.Y) &&
                     (ball.position.Y > (paddle.position.Y - radius - paddle.Height / 2)))
                {
                    ball.direction = Vector2.Reflect(ball.direction, new Vector2(0.196f, -0.981f));
                    colTimer = 60;
                    ballBounceSFX.Play();

                }
                //left collision
                if ((ball.position.X > (paddle.position.X - radius - paddle.Width / 2)) &&
                     (ball.position.X < (paddle.position.X - paddle.Width / 6)) &&
                     (ball.position.Y < paddle.position.Y) &&
                     (ball.position.Y > (paddle.position.Y - radius - paddle.Height / 2)))
                {
                    ball.direction = Vector2.Reflect(ball.direction, new Vector2(-0.196f, -0.981f));
                    ball.colTimer = 60;
                    ballBounceSFX.Play();
                }

                //if ball catch is on and the ball hit the paddle, mark the ball as caught
                if (ball.colTimer == 60 && ballCatch && !ball.caught)
                {
                    ball.caught = true;
                    ball.tempBallPaddleRatio = ball.position - paddle.position;
                }




                bool destroyBlock = false;
                

                foreach (Block b in blocks) {
                    //block collision

                    //left
                    if ((ball.position.X > b.position.X - radius - b.Width / 2) &&
                         (ball.position.X < b.position.X - b.Width / 2) &&
                         (ball.position.Y < b.position.Y + b.Height / 2 - 4) &&
                         (ball.position.Y > b.position.Y - b.Height / 2 + 4))
                    {
                        ball.direction = Vector2.Reflect(ball.direction, new Vector2(-1, 0));
                        blockToDestroy = b;
                        destroyBlock = true;
                        ballHitSFX.Play();
                        break;

                    }
                    //right
                    if ((ball.position.X > b.position.X + b.Width / 2) &&
                         (ball.position.X < b.position.X + radius + b.Width / 2) &&
                         (ball.position.Y < b.position.Y + b.Height / 2 - 4) &&
                         (ball.position.Y > b.position.Y - b.Height / 2 + 4))
                    {
                        ball.direction = Vector2.Reflect(ball.direction, new Vector2(1, 0));
                        blockToDestroy = b;
                        destroyBlock = true;
                        ballHitSFX.Play();
                        break;

                    }
                    //top
                    if ((ball.position.X > b.position.X - b.Width / 2) &&
                         (ball.position.X < b.position.X + b.Width / 2) &&
                         (ball.position.Y < b.position.Y - b.Height / 2) &&
                         (ball.position.Y > b.position.Y - radius - b.Height / 2))
                    {
                        ball.direction = Vector2.Reflect(ball.direction, new Vector2(0, -1));
                        blockToDestroy = b;
                        destroyBlock = true;
                        ballHitSFX.Play();
                        break;

                    }
                    //bottom
                    if ((ball.position.X > b.position.X - b.Width / 2) &&
                         (ball.position.X < b.position.X + b.Width / 2) &&
                         (ball.position.Y < b.position.Y + radius + b.Height / 2) &&
                         (ball.position.Y > b.position.Y + b.Height / 2))
                    {
                        ball.direction = Vector2.Reflect(ball.direction, new Vector2(0, 1));
                        blockToDestroy = b;
                        destroyBlock = true;
                        ballHitSFX.Play();
                        break;

                    }

                }

                if (destroyBlock)
                {
                    
                    if (random.NextDouble()%1 < spawnChance)
                    {
                        SpawnPowerUp(blockToDestroy.position);
                    }
                    blocks.Remove(blockToDestroy);
                    destroyBlock = false;
                }

            }
            
            //kill powerups off screen
            for (int i = powerups.Count - 1; i >= 0; i--)
            {
                if (powerups[i].position.Y > powerups[i].Height / 2 + 768)
                {
                    powerups[i].readyToDestroy = true;
                    destroyPowerUp = true;
                }
            }

            //if we want to destroy a powerup
            if (destroyPowerUp)
            {
                for (int i = powerups.Count - 1; i >= 0; i--)
                {
                    if (powerups[i].readyToDestroy)
                    {
                        PowerUp temppowerup = powerups[i];
                        powerups.Remove(temppowerup);
                    }
                }
                destroyPowerUp = false;
            }


            //Wall Collision
            if (Math.Abs(ball.position.X - 32) < radius)
            {
                ball.direction.X = -ball.direction.X;
            }
            if (Math.Abs(ball.position.X - 992) < radius)
            {
                ball.direction.X = -ball.direction.X;
            }
            if (Math.Abs(ball.position.Y - 32) < radius)
            {
                ball.direction.Y = -ball.direction.Y;
            }
            if (Math.Abs(ball.position.Y) > radius + 768)
            {
                ball.destroy = true;
            }
            

        }





        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            bgTexture = Content.Load<Texture2D>("bg");

            //load paddle
            paddle = new Paddle(this);
            paddle.LoadContent();
            paddle.position = new Vector2(512, 740);

            //load ball
            SpawnBall();

            //sfx
            ballBounceSFX = Content.Load<SoundEffect>("ball_bounce");
            ballHitSFX = Content.Load<SoundEffect>("ball_hit");
            powerUpSFX = Content.Load<SoundEffect>("powerup");
            deathSFX = Content.Load<SoundEffect>("death");

            //blocks
            for (int i = 0; i != blockLayout.GetLength(0); i++) 
            {
                for (int j = 0; j != blockLayout.GetLength(1); j++) {
                    Block tempBlock = new Block((Block.BlockColor)blockLayout[i, j], this);
                    tempBlock.LoadContent();
                    tempBlock.position = new Vector2(64 + j * 64, 100 + i * 32);
                    blocks.Add(tempBlock);
                }
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            paddle.Update(deltaTime);
            foreach (Ball ball in balls)
            {
                ball.Update(deltaTime);
                if (ball.colTimer != 0)
                {
                    ball.colTimer -= 1;
                }
            }

            DestroyBalls();
            
            
            
            foreach (PowerUp powerup in powerups)
            {
                CheckForPowerUps(powerup);
                powerup.Update(deltaTime);
            }

            foreach (Ball ball in balls)
            {
                CheckCollisions(ball);
            
                if (ball.caught)
                {
                    ball.position = paddle.position + ball.tempBallPaddleRatio;
                    KeyboardState keyState = Keyboard.GetState();
                    if (keyState.IsKeyDown(Keys.Space)) {
                        ball.caught = false;
                        if (ball.position.X > paddle.position.X)
                        {
                            ball.direction = new Vector2(.707f, -.707f);
                        }
                        if (ball.position.X < paddle.position.X)
                        {
                            ball.direction = new Vector2(-.707f, -.707f);
                        }
                    }
                }
            }

            if (balls.Count == 0)
            {
                LoseLife();
            }
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.DarkGreen);

            spriteBatch.Begin();

            spriteBatch.Draw(bgTexture, new Vector2(0, 0), Color.Green);
            paddle.Draw(spriteBatch);
            foreach (Ball ball in balls)
            {
                ball.Draw(spriteBatch);
            }
            //draw all powerups
            foreach (PowerUp powerup in powerups)
            {
                powerup.Draw(spriteBatch);
            }
            //draw all blocks
            foreach (Block b in blocks)
            {
                b.Draw(spriteBatch);
            }
            spriteBatch.End();


            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }
        
    }
}
