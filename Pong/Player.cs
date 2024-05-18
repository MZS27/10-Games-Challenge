using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    internal class Player
    {
        const int WINNING_SCORE = 7;
        public Vector2 Position;
        public int Score = 0;
        public float Center
        {
            get { return Position.Y + HEIGHT / 2; }
        }
        public Rectangle Rect
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, WIDTH, HEIGHT); }
        }

        public const int HEIGHT = 64;
        public const int WIDTH = 16;
        public const int SPEED = 400;

        public Player(Vector2 position)
        {
            this.Position = position;
        }

        public Boolean hasWon()
        {
            return Score == WINNING_SCORE;
        }

        public void resetScore()
        {
            Score = 0;
        }
    }

    
}
