

using Microsoft.Xna.Framework;

namespace Breakout
{
    internal class Box : GameObject
    {
        
        public int MARGIN = 10;
        public int LeftBound { get { return Rect.Left + MARGIN; } }
        public int RightBound { get { return Rect.Right - MARGIN; } }
        public int UpperBound { get { return Rect.Top + MARGIN; } }
        public int LowerBound { get { return Rect.Bottom - MARGIN; } }

        public Box()
        {
            WIDTH = 380;
            HEIGHT = 680;
            Position = new Vector2(GameManager.WINDOW_WIDTH / 2 - WIDTH / 2 - MARGIN, GameManager.WINDOW_HEIGHT / 2 - HEIGHT / 2 - MARGIN);
        }

    }
}
