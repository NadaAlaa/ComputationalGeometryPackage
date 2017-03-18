using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            points = points.OrderBy(p => p.X).ToList();
            outPoints = recursiveMergeHull(points);
        }
        int compare(Point a, Point b)
        {
            if (a.X < b.X || a.X == b.X && a.Y < b.Y) return 1;
            return 0;
        }
        List<Point> recursiveMergeHull(List<Point> points)
        {
            if (points.Count <= 2) return points;
            List<Point> left = recursiveMergeHull(points.GetRange(0, points.Count / 2));
            List<Point> right = recursiveMergeHull(points.GetRange(points.Count / 2, (points.Count + 1) / 2));
            List<Point> merged = new List<Point>();

            Line lowerTangent = HelperMethods.GetTangent(left, right, 1);
            Line upperTangent = HelperMethods.GetTangent(left, right, -1);

            int idx = left.FindIndex(p => p == lowerTangent.Start);
            while (left[idx] != upperTangent.Start)
            {
                merged.Add(left[idx]);
                idx = (idx + 1) % left.Count;
            }
            merged.Add(upperTangent.Start);
            idx = right.FindIndex(p => p == upperTangent.End);
            while (right[idx] != lowerTangent.End)
            {
                merged.Add(right[idx]);
                idx = (idx + 1) % right.Count;
            }
            merged.Add(lowerTangent.End);
            return merged;
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}
