using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dooz
{
    class Node
    {
        public int Number { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        private int state = 0;
        public int State
        {
            get => state;
            set
            {
                state = (value >= 0 && value <= 2) ? value : 0;
            }
        }
    }
}
