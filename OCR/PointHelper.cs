using OCR.Models;
using System.Drawing;

namespace OCR
{
    public static class PointHelper
    {
        public static double MeasureDistance(Point point1, Point point2)
            => Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));

        public static PointModel PointBetween(PointModel point1, PointModel point2)
            => new PointModel((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
    }
}
