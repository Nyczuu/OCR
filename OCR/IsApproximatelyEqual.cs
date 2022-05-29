using OCR.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace OCR.Services
{
    public class IsPointFApproximatelyEqual : IEqualityComparer<PointF>
    {
        public bool Equals(PointF first, PointF second)
        {
            int firstX = first.X.RoundToFives();
            int secondX = second.X.RoundToFives();
            int firstY = second.Y.RoundToFives();
            int secondY = second.Y.RoundToFives();
            return firstX == secondX && firstY == secondY;
        }
           

        public int GetHashCode([DisallowNull] PointF obj)
        {
            return obj.X.RoundToFives().GetHashCode() ^ obj.Y.RoundToFives().GetHashCode();
        }
    }

    public class IsPointApproximatelyEqual : IEqualityComparer<Point>
    {
        public bool Equals(Point first, Point second)
        {
            int firstX = first.X.RoundToFives();
            int secondX = second.X.RoundToFives();
            int firstY = second.Y.RoundToFives();
            int secondY = second.Y.RoundToFives();
            return firstX == secondX && firstY == secondY;
        }


        public int GetHashCode([DisallowNull] Point obj)
        {
            return obj.X.RoundToFives().GetHashCode() ^ obj.Y.RoundToFives().GetHashCode();
        }
    }
}