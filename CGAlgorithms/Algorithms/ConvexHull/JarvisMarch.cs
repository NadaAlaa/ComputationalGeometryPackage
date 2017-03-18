using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            int start = 0, end = -1, finish = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < points[start].Y) start = finish = i;
            }
            outPoints.Add(points[start]);
            while (end != finish)
            {
                double minAngle = 360;
                int nextPoint = finish;
                Line a = new Line(points[start], new Point(points[start].X + 10, points[start].Y));
                for (int i = 0; i < points.Count; i++)
                {
                    if (i == start || i == end) continue;

                    Line b = new Line(a.Start, points[i]);
                    if (end != -1)
                    {
                        a = new Line(points[start], points[end]);
                        b = new Line(a.End, points[i]);
                    }
                    double angle = HelperMethods.GetAngle(a, b);
                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        nextPoint = i;
                    }
                }
                if (end != -1 && HelperMethods.PointOnSegment(points[end], points[start], points[nextPoint]))
                {
                    outPoints.Remove(points[end]);
                }
                start = end == -1 ? start : end;
                end = nextPoint;
                if (end != finish) outPoints.Add(points[end]);
            }
        }
        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
