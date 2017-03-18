using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;

namespace CGAlgorithms.Algorithms.Intersection
{
    class LineVsPolygon:Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            for (int i = 0; i < polygons.Count; i++)
            {
                for (int j = 0; j < polygons[i].lines.Count; j++)
                {
                    for (int k = 0; k < lines.Count; k++)
                    {
                        List<Line> a = new List<Line>{lines[k], polygons[i].lines[j]};

                        LineVsLine lineIntersection = new LineVsLine();
                        lineIntersection.Run(new List<Point>(), a, new List<Polygon>(), ref outPoints, ref outLines, ref outPolygons);
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Line Vs Polygon Intersection";
        }
    }
}
