using System.Drawing;

namespace OCR
{
    public static class PointHelper
    {
        public static double MeasureDistance(Point point1, Point point2)
            => Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
    }
}
