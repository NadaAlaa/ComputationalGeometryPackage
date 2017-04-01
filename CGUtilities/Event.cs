using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class Event
    {
        public static List<Line> lines;
        public Point point;
        public Event prev;
        public Event next;
        public int index;
        public int index_aux;
        public Enums.EventType eventType;

        public Event(Point p, int index, Enums.EventType eventType)
        {
            this.point = p;
            this.index = index;
            this.eventType = eventType;
        }
        public Event(Point p, int index, int index_aux, Enums.EventType eventType)
        {
            this.point = p;
            this.index = index;
            this.index_aux = index_aux;
            this.eventType = eventType;
        }
        public Event(Point p, Event prev, Event next, Enums.EventType eventType)
        {
            this.point = p;
            this.prev = prev;
            this.next = next;
            this.eventType = eventType;
        }
    }
}
