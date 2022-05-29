using Emgu.CV.Structure;
using System.Drawing;

namespace OCR
{
    public struct Arrow
    {
        public ICollection<LineSegment2D> Head { get; }
        public ICollection<LineSegment2D> Tail { get; }

        public Point TailBegin { get; }
        public Point TailEnd { get; }

        public Arrow(ICollection<LineSegment2D> head, ICollection<LineSegment2D> tail)
        {
            Head = head;
            Tail = tail;

            TailBegin = tail.First().P1;
            TailEnd = tail.First().P2;
        }
    }
}
