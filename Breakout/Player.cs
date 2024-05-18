using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout
{
    internal class Player : GameObject
    {
        public int Score = 0;
        public int Speed = 300;
        public Player(Vector2 position)
        {
            Position = position;
            HEIGHT = 16;
            WIDTH = 56;
            Position.Y -= HEIGHT / 2; 
            Position.X -= WIDTH / 2;
        }
    }
}
