using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class HelperMethods
    {
        public static List<Point> sortClockWise(List<Point> set)
        {
            if (set.Count > 1)
            {
                //Declarations
                int indexNoClock, indexClock, prev_index;
                const int initial = 0;
                int setSize = set.Count;
                List<Point> setClockWise = new List<Point>();
                Stack<Point> lowerHalfClock = new Stack<Point>();

                //Subsets have to be sorted in x-value ascending to able to sort clock wise.
                set = set.OrderBy(p => p.X).ToList();

                //Setting up certain variables
                setClockWise.Add(set[initial]);
                prev_index = initial;
                indexNoClock = (prev_index + 1) % setSize;

                // Going over every point of the input set and building the upper-half clockwise-sorted set
                //Looking for next point which makes a right turn from setClockWise[indexClock]
                //At the end of this loop:
                //prev_index will be the index in set which corresponds to the last point inserted in setClockWise
                //indexNoClock will be the index for the next point to look at in set
                //indexClock will be incremented by one
                //setClockWise[indexClock] will be updated to the next clockwise direction point
                bool doRightTurn = false;
                while (indexNoClock != (setSize - 1))
                {
                    doRightTurn = (CheckTurn(new Line(set[prev_index], set[indexNoClock]), set[indexNoClock + 1]) == Enums.TurnType.Right);

                    if (doRightTurn)
                    {
                        setClockWise.Add(set[indexNoClock]);
                        prev_index = indexNoClock;
                    }
                    else
                        lowerHalfClock.Push(set[indexNoClock]);
                    ++indexNoClock;
                }

                //Inserting rightmost point i.e. if (indexNoClock == (setSize-1))
                setClockWise.Add(set[indexNoClock]);

                //The rest of the clockwise-sorted set is the lower half taken in reverse, so this can be easily added,
                //using the lowerHalfClock stack, previously used:
                while (lowerHalfClock.Count != 0)
                {
                    setClockWise.Add(lowerHalfClock.Peek());
                    lowerHalfClock.Pop();
                }

                return setClockWise;
            }
            return set;
        }
        public static Line GetTangent(List<Point> left, List<Point> right, int dir)
        {
            int rightMost = 0, leftMost = 0;
            for (int i = 1; i < left.Count; i++)
            {
                if (left[i].X > left[rightMost].X) rightMost = i;
                else if (left[i].X == left[rightMost].X && left[i].Y < left[rightMost].Y) rightMost = i;
            }
            for (int i = 1; i < right.Count; i++)
            {
                if (right[i].X < right[leftMost].X) leftMost = i;
                else if (right[i].X == right[leftMost].X && right[i].Y < right[leftMost].Y) leftMost = i;
            }
            if (dir == 1)
            {
                while (HelperMethods.CheckTurn(new Line(left[rightMost], right[leftMost]), right[(leftMost + 1) % right.Count]) == Enums.TurnType.Right
                    || HelperMethods.CheckTurn(new Line(right[leftMost], left[rightMost]), left[(rightMost - 1 + left.Count) % left.Count]) == Enums.TurnType.Left)
                {
                    while (HelperMethods.CheckTurn(new Line(left[rightMost], right[leftMost]), right[(leftMost + 1) % right.Count]) == Enums.TurnType.Right)
                    {
                        leftMost = (leftMost + 1) % right.Count;
                    }
                    while (HelperMethods.CheckTurn(new Line(right[leftMost], left[rightMost]), left[(rightMost - 1 + left.Count) % left.Count]) == Enums.TurnType.Left)
                    {
                        rightMost = (rightMost - 1 + left.Count) % left.Count;
                    }
                }
                while (HelperMethods.CheckTurn(new Line(left[rightMost], right[leftMost]), right[(leftMost + 1) % right.Count]) == Enums.TurnType.Colinear
                    && right[(leftMost + 1) % right.Count].X > right[leftMost].X) leftMost = (leftMost + 1) % right.Count;
                while (HelperMethods.CheckTurn(new Line(right[leftMost], left[rightMost]), left[(rightMost - 1 + left.Count) % left.Count]) == Enums.TurnType.Colinear
                    && left[(rightMost - 1 + left.Count) % left.Count].X < left[rightMost].X) rightMost = (rightMost - 1 + left.Count) % left.Count;
                
                while (HelperMethods.CheckTurn(new Line(left[rightMost], right[leftMost]), right[(leftMost + 1) % right.Count]) == Enums.TurnType.Colinear
                    && right[(leftMost + 1) % right.Count].Y < right[leftMost].Y) leftMost = (leftMost + 1) % right.Count;
                while (HelperMethods.CheckTurn(new Line(right[leftMost], left[rightMost]), left[(rightMost - 1 + left.Count) % left.Count]) == Enums.TurnType.Colinear
                    && left[(rightMost - 1 + left.Count) % left.Count].Y < left[rightMost].Y) rightMost = (rightMost - 1 + left.Count) % left.Count;
            }
            else
            {
                while (HelperMethods.CheckTurn(new Line(left[rightMost], right[leftMost]), right[(leftMost - 1 + right.Count) % right.Count]) == Enums.TurnType.Left
                    || HelperMethods.CheckTurn(new Line(right[leftMost], left[rightMost]), left[(rightMost + 1) % left.Count]) == Enums.TurnType.Right)
                {
                    while (HelperMethods.CheckTurn(new Line(left[rightMost], right[leftMost]), right[(leftMost - 1 + right.Count) % right.Count]) == Enums.TurnType.Left)
                    {
                        leftMost = (leftMost - 1 + right.Count) % right.Count;
                    }
                    while (HelperMethods.CheckTurn(new Line(right[leftMost], left[rightMost]), left[(rightMost + 1) % left.Count]) == Enums.TurnType.Right)
                    {
                        rightMost = (rightMost + 1) % left.Count;
                    }
                }
                while (HelperMethods.CheckTurn(new Line(left[rightMost], right[leftMost]), right[(leftMost - 1 + right.Count) % right.Count]) == Enums.TurnType.Colinear
                    && right[leftMost].X < right[(leftMost - 1 + right.Count) % right.Count].X) leftMost = (leftMost - 1 + right.Count) % right.Count;
                while (HelperMethods.CheckTurn(new Line(right[leftMost], left[rightMost]), left[(rightMost + 1) % left.Count]) == Enums.TurnType.Colinear
                    && left[rightMost].X > left[(rightMost + 1) % left.Count].X) rightMost = (rightMost + 1) % left.Count;

                while (HelperMethods.CheckTurn(new Line(left[rightMost], right[leftMost]), right[(leftMost - 1 + right.Count) % right.Count]) == Enums.TurnType.Colinear
                    && right[leftMost].Y < right[(leftMost - 1 + right.Count) % right.Count].Y) leftMost = (leftMost - 1 + right.Count) % right.Count;
                while (HelperMethods.CheckTurn(new Line(right[leftMost], left[rightMost]), left[(rightMost + 1) % left.Count]) == Enums.TurnType.Colinear
                    && left[rightMost].Y < left[(rightMost + 1) % left.Count].Y) rightMost = (rightMost + 1) % left.Count;
            }
            return new Line(left[rightMost], right[leftMost]);
        }
        public static Enums.PointInPolygon PointInTriangle(Point p, Point a, Point b, Point c)
        {
            if (a.Equals(b) && b.Equals(c))
            {
                if (p.Equals(a) || p.Equals(b) || p.Equals(c))
                    return Enums.PointInPolygon.OnEdge;
                else
                    return Enums.PointInPolygon.Outside;
            }

            Line ab = new Line(a, b);
            Line bc = new Line(b, c);
            Line ca = new Line(c, a);

            if (GetVector(ab).Equals(Point.Identity)) return (PointOnSegment(p, ca.Start, ca.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (GetVector(bc).Equals(Point.Identity)) return (PointOnSegment(p, ca.Start, ca.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (GetVector(ca).Equals(Point.Identity)) return (PointOnSegment(p, ab.Start, ab.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;

            if (CheckTurn(ab, p) == Enums.TurnType.Colinear)
                return PointOnSegment(p, a, b) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (CheckTurn(bc, p) == Enums.TurnType.Colinear && PointOnSegment(p, b, c))
                return PointOnSegment(p, b, c) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (CheckTurn(ca, p) == Enums.TurnType.Colinear && PointOnSegment(p, c, a))
                return PointOnSegment(p, a, c) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;

            if (CheckTurn(ab, p) == CheckTurn(bc, p) && CheckTurn(bc, p) == CheckTurn(ca, p))
                return Enums.PointInPolygon.Inside;
            return Enums.PointInPolygon.Outside;
        }
        public static Enums.TurnType CheckTurn(Point vector1, Point vector2)
        {
            double result = CrossProduct(vector1, vector2);
            if (result < 0) return Enums.TurnType.Right;
            else if (result > 0) return Enums.TurnType.Left;
            else return Enums.TurnType.Colinear;
        }
        public static double CrossProduct(Point a, Point b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
        public static double DotProduct(Point a, Point b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        public static double GetAngle(Line a, Line b)
        {
            Point vector_1 = new Point(a.End.X - a.Start.X, a.End.Y - a.Start.Y);
            Point vector_2 = new Point(b.End.X - b.Start.X, b.End.Y - b.Start.Y);
            double cross = HelperMethods.CrossProduct(vector_1, vector_2);
            double dot = HelperMethods.DotProduct(vector_1, vector_2);
            double angle = Math.Atan2(cross, dot);
            if (angle < 0) angle += 360;
            return angle;
        }
        public static bool PointOnRay(Point p, Point a, Point b)
        {
            if (a.Equals(b)) return true;
            if (a.Equals(p)) return true;
            var q = a.Vector(p).Normalize();
            var w = a.Vector(b).Normalize();
            return q.Equals(w);
        }
        public static bool PointOnSegment(Point p, Point a, Point b)
        {
            if (a.Equals(b))
                return p.Equals(a);

            if (b.X == a.X)
                return p.X == a.X && (p.Y >= Math.Min(a.Y, b.Y) && p.Y <= Math.Max(a.Y, b.Y));
            if (b.Y == a.Y)
                return p.Y == a.Y && (p.X >= Math.Min(a.X, b.X) && p.X <= Math.Max(a.X, b.X));
            double tx = (p.X - a.X) / (b.X - a.X);
            double ty = (p.Y - a.Y) / (b.Y - a.Y);

            return (Math.Abs(tx - ty) <= Constants.Epsilon && tx <= 1 && tx >= 0);
        }
        /// <summary>
        /// Get turn type from cross product between two vectors (l.start -> l.end) and (l.end -> p)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Enums.TurnType CheckTurn(Line l, Point p)
        {
            Point a = l.Start.Vector(l.End);
            Point b = l.End.Vector(p);
            return HelperMethods.CheckTurn(a, b);
        }
        public static Point GetVector(Line l)
        {
            return l.Start.Vector(l.End);
        }
    }
}
