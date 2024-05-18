
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D paddleTexture;
        Texture2D ballTexture;

        //Vector2 paddlePosition;
        Vector2 ballPosition;
        Vector2 ballDirection;

        KeyboardState keyboard;
        int windowHeight;
        int windowWidth;

        const int PADDLE_HEIGHT = 64;
        const int PADDLE_SPEED = 600;
        const float BALL_SPEED = 250;
        const int PB_WIDTH = 16; // PB : Paddle & Ball
        const int PADDLE_MARGIN = 72;
        const int COMPUTER_PADDLE_SPEED = 250;

        Rectangle ballRect;
        
        double previousPaddleHit = 0f; // fixes an issue with the ball hitting the paddle edge by keeping a timer for the collision

        SpriteFont digitsFont;
        SpriteFont textFont;

        const int FONT_WIDTH = 45 * 7 / 9;
        const int SCORE_MARGIN = 40; // Distance of the score from the center
        Vector2 score1Pos;
        Vector2 score2Pos;
        Vector2 playAgainTextPos;
        Vector2 winnerTextPos;

        Player player1;
        Player player2;

        float ballCenter;
        bool gameIsOver = false;
        string playAgainMessage = "Press Space to Play Again";
        string winnerMessage = "You Won!";

        SoundEffect paddleEffect;
        SoundEffect scoreEffect;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
			//_graphics.IsFullScreen = true;
			Content.RootDirectory = "Content";
            IsMouseVisible = true;
            windowHeight = Window.ClientBounds.Height;
            windowWidth = Window.ClientBounds.Width;
            Vector2 player1Position = new Vector2(PADDLE_MARGIN, (windowHeight / 2) - (PADDLE_HEIGHT / 2));
            Vector2 player2Position = new Vector2(windowWidth - PADDLE_MARGIN, (windowHeight / 2) - (PADDLE_HEIGHT / 2));
            

            player1 = new Player(player1Position);
            player2 = new Player(player2Position);

            score1Pos = new Vector2((windowWidth / 2) - SCORE_MARGIN - FONT_WIDTH, SCORE_MARGIN);
            score2Pos = new Vector2((windowWidth / 2) + SCORE_MARGIN, SCORE_MARGIN);
            
        }

        protected override void Initialize()
        {
            startBallMovement(-1);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            paddleTexture = Content.Load<Texture2D>("PongPaddle"); 
            ballTexture = Content.Load<Texture2D>("PongBall");
            digitsFont = Content.Load<SpriteFont>("PongDigits");
            textFont = Content.Load<SpriteFont>("pixelFont");

            paddleEffect = Content.Load<SoundEffect>("PaddleEffect");
            scoreEffect = Content.Load<SoundEffect>("ScoreEffect");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (gameIsOver)
            {
                if (keyboard.IsKeyDown(Keys.Space))
                {
                    restartGame();
                }
            }
            ballCenter = ballPosition.Y + PB_WIDTH / 2;

            player1Input(gameTime);
            player2AI(gameTime);
            ballMovement(gameTime);

            base.Update(gameTime);
        }

        private void player1Input(GameTime gameTime)
        {
            if (keyboard.IsKeyDown(Keys.W))
            {
                player1.Position.Y -= PADDLE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboard.IsKeyDown(Keys.S))
            {
                player1.Position.Y += PADDLE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            player1.Position = checkPaddleBounds(player1.Position);
        }
        
        private void player2Input(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Up))
            {
                player2.Position.Y -= PADDLE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                player2.Position.Y += PADDLE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            player2.Position = checkPaddleBounds(player2.Position);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            _spriteBatch.Begin();

            _spriteBatch.DrawString(digitsFont, "" + player1.Score, score1Pos, Color.White);
            _spriteBatch.DrawString(digitsFont, "" + player2.Score, score2Pos, Color.White);

            if (gameIsOver)
            {
                _spriteBatch.DrawString(textFont, playAgainMessage, playAgainTextPos, Color.White);
                _spriteBatch.DrawString(textFont, winnerMessage, winnerTextPos, Color.White);
                _spriteBatch.End();
                base.Draw(gameTime);
                return;
            }

            for (int yPos = PB_WIDTH * 5 / 2; yPos < windowHeight - PB_WIDTH * 2; yPos += PB_WIDTH * 2)
            {
                _spriteBatch.Draw(ballTexture, new Vector2(windowWidth / 2 - PB_WIDTH / 2, yPos), Color.White);
            }

            _spriteBatch.Draw(paddleTexture, player1.Position, Color.White);
            _spriteBatch.Draw(paddleTexture, player2.Position, Color.White);
            _spriteBatch.Draw(ballTexture, ballPosition, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void startBallMovement(int xDirection) // xDirection : -1 towards player, 1 towards computer
        {
            ballPosition = new Vector2((windowWidth / 2) - (PB_WIDTH / 2), (windowHeight / 2) - (PB_WIDTH / 2));
            ballRect = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, PB_WIDTH, PB_WIDTH);

            Random random = new Random();

            if(xDirection == 0)
            {
                ballDirection.X = random.Next(0, 2) == 0 ? -1 : 1;
            }
            else
            {
                ballDirection.X = xDirection;
            }
            ballDirection.Y = random.Next(0, 2) == 0 ? -0.5f : 0.5f;
        }

        void ballMovement(GameTime gameTime)
        {
            
            ballPosition.X += ballDirection.X * BALL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            ballPosition.Y += ballDirection.Y * BALL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

            ballRect.X = (int)ballPosition.X;
            ballRect.Y = (int)ballPosition.Y;

            if (ballRect.Intersects(player1.Rect) && (gameTime.TotalGameTime.TotalSeconds - previousPaddleHit) > 1)
            {
                paddleEffect.Play(0.35f, 0f, 0f);
                ballDirection.X *= -1;
                ballDirection.Y = (player1.Center - ballCenter) / (PADDLE_HEIGHT / 4) * -1;
                
                previousPaddleHit = gameTime.TotalGameTime.TotalSeconds;
            }
            else if(ballRect.Intersects(player2.Rect) && (gameTime.TotalGameTime.TotalSeconds - previousPaddleHit) > 1) 
            {
                paddleEffect.Play(0.35f, 0f, 0f);
                ballDirection.X *= -1;
                ballDirection.Y = (player2.Center - ballCenter) / (PADDLE_HEIGHT/4) * -1;
                
                previousPaddleHit = gameTime.TotalGameTime.TotalSeconds;
            }
            if ( ballPosition.Y <= 0 || (ballPosition.Y + PB_WIDTH) >= windowHeight)
            {
                ballDirection.Y *= -1;
            }
            checkBallOutOfBounds();
        }

        public void checkBallOutOfBounds()
        {
            if(ballPosition.X + PB_WIDTH < 0)
            {
                player2.Score++;
                scoreEffect.Play();
                if (player2.hasWon()) 
                {
                    gameOver();
                    return;
                }
                startBallMovement(1); // 1 : ball goes towards player 2
            }
            else if(ballPosition.X > windowWidth)
            {
                player1.Score++;
                scoreEffect.Play();
                if (player1.hasWon()) 
                {
                    gameOver();
                    return;
                }
                startBallMovement(-1); // -1 : ball goes towards player 1
            }
        }

        public  void gameOver()
        {
            if (player2.hasWon()) 
            {
                winnerMessage = "Computer Wins!";
            }
            playAgainTextPos = new Vector2((windowWidth / 2) - textFont.MeasureString(playAgainMessage).X / 2, (windowHeight / 2) - textFont.MeasureString(playAgainMessage).Y / 2);
            winnerTextPos = new Vector2((windowWidth / 2) - textFont.MeasureString(winnerMessage).X / 2, (windowHeight / 2) - textFont.MeasureString(winnerMessage).Y * 2);
            ballPosition.X = windowWidth / 2;
            ballPosition.Y = windowHeight / 2;
            ballDirection = Vector2.Zero;
            gameIsOver = true;
        }
        public void player2AI(GameTime gameTime)
        {
            if(ballPosition.X < windowWidth / 2)
                return; // computer should only move when the ball is in its play area
            if(player2.Center > ballCenter + PADDLE_HEIGHT / 2)
            {
                player2.Position.Y -= COMPUTER_PADDLE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(player2.Center < ballCenter - PADDLE_HEIGHT / 2)
            {
                player2.Position.Y += COMPUTER_PADDLE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            player2.Position = checkPaddleBounds(player2.Position);
        }

        public Vector2 checkPaddleBounds(Vector2 position) 
        {
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            else if(position.Y > windowHeight - PADDLE_HEIGHT)
            {
                position.Y = windowHeight - PADDLE_HEIGHT;
            }
            return position;
        }
        public void restartGame()
        {
            startBallMovement(0);
            player1.resetScore();
            player2.resetScore();
            gameIsOver = false;
        }
    }
}
