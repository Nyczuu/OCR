using OCR.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace OCR.Services
{
    internal class IsApproximatelyEqual : IEqualityComparer<PointF>
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
}