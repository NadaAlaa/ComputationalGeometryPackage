using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
using CGUtilities.DataStructures;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    class SweepLine : Algorithm
    {
        public class EventComparer
        {
            public static int CompareY(Event x, Event y)
            {
                if (y.point.Y > x.point.Y) return -1;
                if (y.point.Y < x.point.Y) return 1;
                if (y.point.X > x.point.X) return -1;
                if (y.point.X < x.point.X) return 1;
                if (Event.lines[y.index].End.Y > Event.lines[x.index].End.Y) return -1;
                if (Event.lines[y.index].End.Y < Event.lines[x.index].End.Y) return 1;
                if (x.index != y.index || y.index_aux != x.index_aux) return -1;
                return 0;
            }
            public static int CompareX(Event x, Event y)
            {
                if (y.point.X > x.point.X) return -1;
                if (y.point.X < x.point.X) return 1;
                if (y.point.Y > x.point.Y) return -1;
                if (y.point.Y < x.point.Y) return 1;
                if (Event.lines[y.index].End.X > Event.lines[x.index].End.X) return -1;
                if (Event.lines[y.index].End.X < Event.lines[x.index].End.X) return 1;
                return 0;
            }
        }
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            Event.lines = lines;
            OrderedSet<Event> Q = new OrderedSet<Event>(EventComparer.CompareX);
            OrderedSet<Event> L = new OrderedSet<Event>(EventComparer.CompareY);
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Start > lines[i].End) HelperMethods.Swap(lines[i].Start, lines[i].End); 
                Q.Add(new Event(lines[i].Start, i, Enums.EventType.StartPoint));
                Q.Add(new Event(lines[i].End, i, Enums.EventType.EndPoint));
            }
            while (Q.Count != 0)
            {
                Event e = Q.GetFirst();
                Q.Remove(e);

                #region Start Point
                if (e.eventType == Enums.EventType.StartPoint)
                {
                    Point intersection = null;
                    if (HelperMethods.IsVertical(lines[e.index]))
                    {
                        IList<Event> toBeCompared = L.AsList();
                        for (int i = 0; i < toBeCompared.Count; i++)
                        {
                            intersection = HelperMethods.GetIntersection(lines[toBeCompared[i].index], lines[e.index]);
                            if(intersection != null)
                            {
                                outPoints.Add(intersection);
                            }
                        }
                        continue;
                    }
                    L.Add(e);
                    KeyValuePair<Event, Event> KVP = L.DirectUpperAndLower(e);
                    Event prev = KVP.Key;
                    Event next = KVP.Value;

                    if (prev != null)
                    {
                        intersection = HelperMethods.GetIntersection(lines[prev.index], lines[e.index]);
                        if (intersection != null) Q.Add(new Event(intersection, prev, e, Enums.EventType.Intersection));
                    }
                    if (next != null)
                    {
                        intersection = HelperMethods.GetIntersection(lines[e.index], lines[next.index]);
                        if (intersection != null) Q.Add(new Event(intersection, e, next, Enums.EventType.Intersection));
                    }
                }
                #endregion
                #region End Point
                else if (e.eventType == Enums.EventType.EndPoint)
                {
                    KeyValuePair<Event, Event> KVP = L.DirectUpperAndLower(e);
                    Event prev = KVP.Key;
                    Event next = KVP.Value;

                    if (prev != null && next != null)
                    {
                        Point intersection = HelperMethods.GetIntersection(lines[prev.index], lines[next.index]);
                        if (intersection != null) Q.Add(new Event(intersection, prev, next, Enums.EventType.Intersection));
                    }

                    L.RemoveAll(p => p.index == e.index);
                }
                #endregion
                #region Intersection Point
                else
                {
                    if(!outPoints.Contains(e.point))
                        outPoints.Add(e.point);
                    Event prev = e.prev;
                    Event next = e.next;
                    Event prev_prev = L.DirectUpperAndLower(prev).Key;
                    Event next_next = L.DirectUpperAndLower(next).Value;
                    Point intersection = null;
                    if (prev_prev != null)
                    {
                        intersection = HelperMethods.GetIntersection(lines[prev_prev.index], lines[next.index]);
                        if (intersection != null)
                            Q.Add(new Event(intersection, prev_prev, next, Enums.EventType.Intersection));
                    }
                    if (next_next != null)
                    {
                        intersection = HelperMethods.GetIntersection(lines[prev.index], lines[next_next.index]);
                        if (intersection != null) 
                            Q.Add(new Event(intersection, prev, next_next, Enums.EventType.Intersection));
                    }
                    prev.point = next.point = e.point;
                    L.RemoveAll(p => p.index == prev.index);
                    L.RemoveAll(p => p.index == next.index);
                    L.Add(next);
                    L.Add(prev);
                }
                #endregion
            }
        }

        public override string ToString()
        {
            return "Sweep Line";
        }
    }
}
