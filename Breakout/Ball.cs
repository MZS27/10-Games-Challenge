using Microsoft.Xna.Framework;
using System;

namespace Breakout
{
    internal class Ball : GameObject
    {
        public int Speed = 500;
        public Vector2 Direction;
        public Ball(Vector2 position)
        {
            HEIGHT = WIDTH = 16;
            Position = position;
            Position.X -= WIDTH / 2;
            Position.Y -= HEIGHT / 2;
        }

        public void startMovement()
        {

            Random random = new Random();

            Direction.X = random.Next(0, 2) == 0 ? -0.5f : 0.5f;
            Direction.Y = -1;
        }

        public void movement(GameTime gameTime)
        {
			Position.X += Direction.X * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
			Position.Y += Direction.Y * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}
    }
}
