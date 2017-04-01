using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class SortedPoint : IComparable
    {
        public double Theta;
        public Point P;

        public SortedPoint(Point p, double theta)
        {
            P = p;
            Theta = theta;
            if (Theta < 0) Theta += 2 * Math.PI;
        }

        public int CompareTo(object j)
        {
            SortedPoint obj = (SortedPoint)j;
            if (obj.Theta > Theta) return -1;
            if (obj.Theta < Theta) return 1;
            if (obj.P.X > P.X) return -1;
            if (obj.P.X < P.X) return 1;
            if (obj.P.Y > P.Y) return -1;
            if (obj.P.Y < P.Y) return 1;
            return 0;
        }
    }
}