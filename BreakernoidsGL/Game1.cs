using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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
        Paddle paddle;
        Ball ball;
        List<Block> blocks = new List<Block>();
        Block blockToDestroy;
        int colTimer = 20;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
        }

        protected void LoseLife()
        {
            paddle.position = new Vector2(512, 740);
            ball.position = new Vector2(paddle.position.X, paddle.position.Y - ball.Height / 2 - paddle.Height / 2);
            ball.direction = new Vector2(.707f, -.707f);
        }

        protected void CheckCollisions()
        {
            float radius = ball.Width / 2;

            //paddle collision
            if (colTimer == 0)
            {
                //center collision
                if ((ball.position.X > (paddle.position.X - paddle.Width / 6)) &&
                     (ball.position.X < (paddle.position.X + paddle.Width / 6)) &&
                     (ball.position.Y < paddle.position.Y) &&
                     (ball.position.Y > (paddle.position.Y - radius - paddle.Height / 2)))
                {
                    ball.direction = Vector2.Reflect(ball.direction, new Vector2(0, -1));
                    colTimer = 60;
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

                }
                //left collision
                if ((ball.position.X > (paddle.position.X - radius - paddle.Width / 2)) &&
                     (ball.position.X < (paddle.position.X - paddle.Width / 6)) &&
                     (ball.position.Y < paddle.position.Y) &&
                     (ball.position.Y > (paddle.position.Y - radius - paddle.Height / 2)))
                {
                    ball.direction = Vector2.Reflect(ball.direction, new Vector2(-0.196f, -0.981f));
                    colTimer = 60;
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
                        break;

                    }

                }

                if (destroyBlock)
                {
                    blocks.Remove(blockToDestroy);
                    destroyBlock = false;
                }
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
                LoseLife();
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
            paddle = new Paddle(this);
            paddle.LoadContent();
            ball = new Ball(this);
            ball.LoadContent();
            paddle.position = new Vector2(512, 740);
            ball.position = new Vector2(512, 740);

            for (int i = 0; i < 15; i++)
            {
                Block tempBlock = new Block(Block.BlockColor.Blue, this);
                tempBlock.LoadContent();
                tempBlock.position = new Vector2(64 + i * 64, 200);
                blocks.Add(tempBlock);
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
            ball.Update(deltaTime);
            
            
            if (colTimer != 0)
            {
                colTimer -= 1;
            }
            CheckCollisions();
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
            ball.Draw(spriteBatch);
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
