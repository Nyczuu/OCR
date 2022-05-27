using Emgu.CV.Structure;

namespace OCR
{
    internal struct Arrow
    {
        public ICollection<LineSegment2D> Lines { get; }

        private Arrow(ICollection<LineSegment2D> lines)
        {
            Lines = lines;
        }

        public static Arrow FromLines(ICollection<LineSegment2D> lines)
        {
            return new Arrow(lines);
        }
    }
}
