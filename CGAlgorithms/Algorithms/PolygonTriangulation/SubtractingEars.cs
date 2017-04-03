using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class SubtractingEars : Algorithm
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
                List<int> ears = FindEars(vertices);
                while (ears.Count != 0 && vertices.Count > 3)
                {
                    outLines.Add(SubtractEar(ears[0], ref ears, ref vertices));
                }
            }
        }
        private Line SubtractEar(int i, ref List<int> ears, ref List<Point> vertices)
        {
            int N = vertices.Count;
            Line diagonal = new Line(vertices[(i - 1 + N) % N], vertices[(i + 1) % N]);
            N--;
            vertices.RemoveAt(i);
            ears.Remove(i);
            for (int j = 0; j < ears.Count; j++) if (ears[j] > i) ears[j]--;

            if (IsEar(i % N, vertices) && !ears.Contains(i % N)) 
                ears.Add(i % N);
            else if (ears.Contains(i % N) && !IsEar(i % N, vertices)) 
                ears.Remove(i % N);

            if (IsEar((i - 1 + N) % N, vertices) && !ears.Contains((i - 1 + N) % N))
                ears.Add((i - 1 + N) % N);
            else if (!IsEar((i - 1 + N) % N, vertices) && ears.Contains((i - 1 + N) % N))
                ears.Remove((i - 1 + N) % N);
            
            return diagonal;
        }
        private List<int> FindEars(List<Point> vertices)
        {
            int N = vertices.Count;
            List<int> ears = new List<int>();
            for (int i = 0; i < N; i++)
                if (IsEar(i, vertices)) ears.Add(i);
            return ears;
        }
        private bool IsEar(int idx, List<Point> vertices)
        {
            int N = vertices.Count;
            if (!HelperMethods.IsConvex(vertices[idx], vertices[(idx - 1 + N) % N], vertices[(idx + 1) % N]))
                return false;
            for (int i = 0; i < N; i++)
            {
                if (i == idx || i == (idx - 1 + N) % N || i == (idx + 1) % N) continue;
                if (HelperMethods.PointInTriangle(vertices[i], vertices[(idx - 1 + N) % N], vertices[idx], vertices[(idx + 1) % N]) != Enums.PointInPolygon.Outside)
                    return false;
            }
            return true;
        }
        public override string ToString()
        {
            return "Subtracting Ears";
        }
    }
}
