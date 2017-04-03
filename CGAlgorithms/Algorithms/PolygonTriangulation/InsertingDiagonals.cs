using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class InsertingDiagonals : Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            for (int i = 0; i < polygons.Count; i++)
            {
                List<Point> vertices = new List<Point>();
                for (int j = 0; j < polygons[i].lines.Count; j++)
                {
                    vertices.Add(polygons[i].lines[j].Start);
                }
                HelperMethods.SortCCW(ref vertices);
                FindDiagonals(vertices, ref outLines);
            }
        }

        private void FindDiagonals(List<Point> vertices, ref List<Line> outLines)
        {
            if (vertices.Count <= 3) return;
            int j = 0, idx_Diagonal_Start = 0;
            int N = vertices.Count;
            List<Point> p1 = new List<Point>(), p2 = new List<Point>();
            Point nearestPoint = null;
            double nearestDist = double.MaxValue;
            for (j = 0; j < N; j++)
            {
                if (HelperMethods.IsConvex(vertices[j], vertices[(j - 1 + N) % N],vertices[(j + 1) % N]))
                {
                    for (int k = 0; k < N; k++)
                    {
                        if (k == j || k == (j + 1) % N || k == (j - 1 + N) % N) continue;
                        if (HelperMethods.PointInTriangle(vertices[k], vertices[(j - 1 + N) % N], vertices[j], vertices[(j + 1) % N])
                            == Enums.PointInPolygon.Inside)
                        {
                            double curDist = HelperMethods.GetDistanceBetweenPoints(vertices[j], vertices[k]);
                            if (curDist < nearestDist)
                            {
                                nearestDist = curDist;
                                nearestPoint = vertices[k];
                            }
                        }
                    }
                    break;
                }
            }
            Line diagonal = null;
            if (nearestPoint == null)
            {
                diagonal = new Line(vertices[(j - 1 + N) % N], vertices[(j + 1) % N]);
                idx_Diagonal_Start = (j - 1 + N) % N;
            }
            else
            {
                diagonal = new Line(vertices[j], nearestPoint);
                idx_Diagonal_Start = j;
            }
            outLines.Add(diagonal);
            Point start = vertices[idx_Diagonal_Start];
            while (start != diagonal.End)
            {
                p1.Add(start);
                idx_Diagonal_Start = (idx_Diagonal_Start + 1) % N;
                start = vertices[idx_Diagonal_Start];
            }
            p1.Add(diagonal.End);
            while (start != diagonal.Start)
            {
                p2.Add(start);
                idx_Diagonal_Start = (idx_Diagonal_Start + 1 + N) % N;
                start = vertices[idx_Diagonal_Start];
            }
            p2.Add(diagonal.Start);
            HelperMethods.SortCCW(ref p1);
            HelperMethods.SortCCW(ref p2);
            FindDiagonals(p1, ref outLines);
            FindDiagonals(p2, ref outLines);
        }

        public override string ToString()
        {
            return "Inserting Diagonals";
        }
    }
}
