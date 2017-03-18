using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            for (int p = 0; p < points.Count; p++)
            {
                bool extreme = true;
                for (int i = 0; i < points.Count; i++)
                {
                    if (i == p) continue;
                    for (int j = i + 1; j < points.Count; j++)
                    {
                        if (j == p) continue;
                        for (int k = j + 1; k < points.Count; k++)
                        {
                            if (k == p) continue;
                            extreme &= HelperMethods.PointInTriangle(points[p], points[i], points[j], points[k]) == Enums.PointInPolygon.Outside;
                            extreme &= !HelperMethods.PointOnSegment(points[p], points[i], points[j]);
                            extreme &= !HelperMethods.PointOnSegment(points[p], points[i], points[k]);
                            extreme &= !HelperMethods.PointOnSegment(points[p], points[k], points[j]);
                        }
                    }
                }
                if (extreme) outPoints.Add(points[p]);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
