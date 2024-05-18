using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout
{
    internal class GameObject
    {
        public int HEIGHT;
        public int WIDTH;
        public Vector2 Position;
        public Rectangle Rect
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, WIDTH, HEIGHT); }
        }
        public Texture2D Sprite;

        public void Render(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(Sprite, Position, Color.White);
        }
    }
}
