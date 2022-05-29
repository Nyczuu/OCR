using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace OCR
{
    public struct Arrow
    {
        public ICollection<LineSegment2D> Head { get; }
        public LineSegment2D Tail { get; }

        public Point TailBegin { get; }
        public Point TailEnd { get; }

        public Arrow(ICollection<LineSegment2D> head, ICollection<LineSegment2D> tail)
        {
            Head = head;
            Tail = tail.First();

            var headPart = head.First();
            var p1Distance = PointHelper.MeasureDistance(Tail.P1, headPart.P1);
            var p2Distance = PointHelper.MeasureDistance(Tail.P2, headPart.P1);

            TailBegin = p1Distance < p2Distance ? Tail.P1 : Tail.P2;
            TailEnd = p1Distance > p2Distance ? Tail.P1 : Tail.P2;
        }
    }
}
