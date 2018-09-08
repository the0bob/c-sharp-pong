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

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameObject topWall;
        GameObject bottomWall;

        GameObject playerOne;
        GameObject playerTwo;
        KeyboardState keyboardState;

        GameObject ball;

        SpriteFont gameFont;

        int playerOneScore = 0;
        int playerTwoScore = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            //Load Font
            gameFont = Content.Load<SpriteFont>("GameFont");

            //Create Walls
            Texture2D wallTexture = Content.Load<Texture2D>("wall");
            
            topWall = new GameObject
            (
                wallTexture,
                Vector2.Zero
            );
            
            bottomWall = new GameObject
            (
                wallTexture,
                new Vector2 (0, Window.ClientBounds.Height - wallTexture.Height)
            );

            //Create Players
            Texture2D paddleTexture = Content.Load<Texture2D>("paddle");
            Vector2 position;

            position = new Vector2
                (
                0,
                Window.ClientBounds.Height - paddleTexture.Height
                );
            playerOne = new GameObject(paddleTexture, position);

            position = new Vector2
                (
                Window.ClientBounds.Width - paddleTexture.Width,
                Window.ClientBounds.Height - paddleTexture.Height
                );
            playerTwo = new GameObject(paddleTexture, position);

            //Create Ball
            Texture2D ballTexture = Content.Load<Texture2D>("ball");

            position = new Vector2
                (
                    playerOne.BoundingBox.Right + 1,
                    (Window.ClientBounds.Height - ballTexture.Height) / 2
                );

            ball = new GameObject
            (
                ballTexture, position, new Vector2(4f, -4f)
            );
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            ball.Position += ball.Velocity;

            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W))
                playerOne.Position.Y -= 10f;

            if (keyboardState.IsKeyDown(Keys.S))
                playerOne.Position.Y += 10f;

            if (keyboardState.IsKeyDown(Keys.Up))
                playerTwo.Position.Y -= 10f;

            if (keyboardState.IsKeyDown(Keys.Down))
                playerTwo.Position.Y += 10f;

            CheckPaddleWallCollision();
            CheckBallCollision();

            base.Update(gameTime);
        }


        private void CheckPaddleWallCollision()
        {
            if (playerOne.BoundingBox.Intersects(topWall.BoundingBox))
            {
                playerOne.Position.Y = topWall.BoundingBox.Bottom;
            }

            if (playerOne.BoundingBox.Intersects(bottomWall.BoundingBox))
            {
                playerOne.Position.Y = bottomWall.BoundingBox.Y 
                    - playerOne.BoundingBox.Height;
            }

            if (playerTwo.BoundingBox.Intersects(topWall.BoundingBox))
            {
                playerTwo.Position.Y = topWall.BoundingBox.Bottom;
            }

            if (playerTwo.BoundingBox.Intersects(bottomWall.BoundingBox))
            {
                playerTwo.Position.Y = bottomWall.BoundingBox.Y 
                    - playerTwo.BoundingBox.Height;
            }
        }

        private void CheckBallCollision() 
        {
            if (ball.BoundingBox.Intersects(topWall.BoundingBox) || ball.BoundingBox.Intersects(bottomWall.BoundingBox))
            {
                ball.Velocity.X *= 1.1f;
                ball.Velocity.Y *= -1.1f;
                ball.Position += ball.Velocity;
            }

            if (ball.BoundingBox.Intersects(playerOne.BoundingBox))
            {
                ball.Velocity.X *= -1.1f;
                ball.Velocity.Y *= 1.1f;
                ball.Position += ball.Velocity;
            }

            if (ball.BoundingBox.Intersects(playerTwo.BoundingBox))
            {
                ball.Velocity.X *= -1.1f;
                ball.Velocity.Y *= 1.1f;
                ball.Position += ball.Velocity;
            }
            
            if (ball.Position.X < 0)
            {
                ball.Position.X = playerOne.BoundingBox.Right + 1;
                ball.Position.Y = playerOne.Position.Y + (playerOne.BoundingBox.Height / 2);
                ball.Velocity = new Vector2(4, -4);
                playerTwoScore += 1;
            }
            
            if (ball.Position.X > Window.ClientBounds.Width)
            {
                ball.Position.X = playerTwo.BoundingBox.Left - ball.BoundingBox.Width - 1;
                ball.Position.Y = playerTwo.Position.Y + (playerTwo.BoundingBox.Height / 2);
                ball.Velocity = new Vector2(-4, 4);
                playerOneScore += 1;
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            topWall.Draw(spriteBatch);
            bottomWall.Draw(spriteBatch);

            playerOne.Draw(spriteBatch);
            playerTwo.Draw(spriteBatch);

            ball.Draw(spriteBatch);


            //Draw Player Scores
            string playerOneScoreString = "Player One Score:" + playerOneScore.ToString();
            spriteBatch.DrawString(
                gameFont,
                playerOneScoreString, 
                new Vector2(32, Window.ClientBounds.Height - 32), 
                Color.Red, 
                0, 
                Vector2.Zero, 
                1, 
                SpriteEffects.None, 
                0);

            string playerTwoScoreString = "Player Two Score:" + playerTwoScore.ToString();
            spriteBatch.DrawString(
                gameFont, 
                playerTwoScoreString, 
                new Vector2(
                    Window.ClientBounds.Width - 32, 
                    Window.ClientBounds.Height - 32), 
                Color.Red, 0, 
                new Vector2(
                    gameFont.MeasureString(playerTwoScoreString).X, 
                    0), 
                1, 
                SpriteEffects.None, 
                0);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
