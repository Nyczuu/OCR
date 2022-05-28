using Emgu.CV.Structure;

namespace OCR
{
    public struct Arrow
    {
        public ICollection<LineSegment2D> Head { get; }
        public ICollection<LineSegment2D> Tail { get; }

        public Arrow(ICollection<LineSegment2D> head, ICollection<LineSegment2D> tail)
        {
            Head = head;
            Tail = tail;
        }
    }
}
