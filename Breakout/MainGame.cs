using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Breakout
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState keyboard;

        public int PADDLE_DISTANCE_CENTER = 200; // Distance on the Y axis
        public int BALL_DISTANCE_CENTER = 200 - 20; // Starting position

        Box box;
        Player player;
        Ball ball;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GameManager.WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = GameManager.WINDOW_HEIGHT;
            _graphics.ApplyChanges();

            
            box = new Box();
            player = new Player(new Vector2(box.Rect.Center.X, box.Rect.Center.Y + PADDLE_DISTANCE_CENTER));
			ball = new Ball(new Vector2(player.Position.X + player.WIDTH/2, player.Position.Y - player.HEIGHT*2));

			Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ball.startMovement();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            box.Sprite = Content.Load<Texture2D>("BreakoutArea");
            player.Sprite = Content.Load<Texture2D>("BreakoutPaddle");
            ball.Sprite = Content.Load<Texture2D>("BreakoutBall");

        }

        protected override void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
                Exit();
            HandleInput(gameTime);
            ball.movement(gameTime);
            checkBallBounds();

            base.Update(gameTime);
        }


        void HandleInput(GameTime gameTime)
        {
            if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.Z))
            {
                player.Position.X -= player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (player.Position.X < box.LeftBound)
                {
                    player.Position.X = box.LeftBound;
                }
            }
            else if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.X))
            {
                player.Position.X += player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (player.Position.X > (box.RightBound - player.WIDTH))
                {
                    player.Position.X = box.RightBound - player.WIDTH;
                }
            }
        }

        public void checkBallBounds()
        {
			if (ball.Position.X < box.LeftBound || ball.Position.X + ball.WIDTH > box.RightBound)
			{
                ball.Direction.X *= -1;
			}
            if (ball.Position.Y + ball.HEIGHT < box.UpperBound) 
			{
                ball.Direction.Y *= -1;
			}
		}
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin();

            box.Render(_spriteBatch);
            player.Render(_spriteBatch);
            ball.Render(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
