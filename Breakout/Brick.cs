using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    internal class Brick : GameObject
    {
        public int score = 1;
        public int collide()
        {
            return score;
        }
    }
}
