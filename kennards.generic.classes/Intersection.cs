using System;
using System.Collections.Generic;
using System.Text;

namespace kennards.generic.classes
{
    public class Intersection
    {
        public Player OccupiedBy { get; set; } = null;
        public int Row { get; set; }
        public int Column { get; set; }
        public Intersection()
        {
            OccupiedBy = new Player();
            Row = -1;
            Column = -1;
        }

        public Intersection(Player P, int R, int C)
        {
            OccupiedBy = P;
            Row = R;
            Column = C;
        }
    }
}
