using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1)
            {
                outPoints = points;
                return;
            }
            int N = points.Count, p0 = 0, p1 = 0, p2 = 0;
            List<int> convexHull = new List<int>();
            List<Tuple<double, int>> innerPoints = new List<Tuple<double, int>>();
            for (int i = 0; i < N; i++)
            {
                if (points[i].Y < points[p0].Y || (points[i].Y == points[p0].Y && points[i].X < points[p0].X)) p0 = i;
            }
            convexHull.Add(p0);
            for (int i = 0; i < N; i++)
            {
                if(i == p0) continue;
                Line a = new Line(points[p0], new Point(points[p0].X+10, points[p0].Y));
                Line b = new Line(points[p0], points[i]);
                double curAngle = HelperMethods.GetAngle(a, b);
                innerPoints.Add(new Tuple<double, int>(curAngle, i));
            }
            innerPoints.Sort();
            convexHull.Add(innerPoints[0].Item2);
            innerPoints.RemoveAt(0);
            for (int i = 0; i < innerPoints.Count; i++)
            {
                N = convexHull.Count - 1;
                p0 = convexHull[N - 1];
                p1 = convexHull[N];
                p2 = innerPoints[i].Item2;
                Line l = new Line(points[p0], points[p1]);
                if (HelperMethods.CheckTurn(l, points[p2]) == Enums.TurnType.Left)
                {
                    convexHull.Add(p2);
                }
                else if(HelperMethods.CheckTurn(l, points[p2]) == Enums.TurnType.Colinear && !HelperMethods.PointOnSegment(points[p2], points[p0], points[p1]))
                {
                    convexHull.Remove(p1);
                    convexHull.Add(p2);
                    i--;
                }
                else if(convexHull.Count > 2)
                {
                    convexHull.Remove(p1);
                    i--;
                }
            }
            for (int i = 0; i < convexHull.Count; i++) outPoints.Add(points[convexHull[i]]);
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
