using Emgu.CV.Structure;
using System.Drawing;

namespace OCR.Models
{
    public class RectangleModel
    {
        public RectangleModel(PointModel center, PointModel topLeft, PointModel topRight, PointModel bottomLeft, PointModel bottomRight)
        {
            Id = Guid.NewGuid();
            Center = center;
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
        }

        public Guid Id { get; }
        public PointModel Center { get; }
        public PointModel TopLeft { get; }
        public PointModel TopRight { get; }
        public PointModel BottomLeft { get; }
        public PointModel BottomRight { get; }

        public static RectangleModel FromRotatedRect(RotatedRect rotatedRect)
        {
            var center = new PointModel(rotatedRect.Center.X, rotatedRect.Center.Y);
            var vertices = rotatedRect.GetVertices();

            return new RectangleModel(center, GetTopLeft(vertices), GetTopRight(vertices), GetBottomLeft(vertices), GetBottomRight(vertices));
        }

        private static PointModel GetTopLeft(PointF[] vertices)
        {
            var mostLeft = vertices.OrderBy(x => x.X).Take(2);
            var topLeft = mostLeft.OrderBy(x => x.Y).First();
            return new PointModel(topLeft.X, topLeft.Y);
        }

        private static PointModel GetTopRight(PointF[] vertices)
        {
            var mostRight = vertices.OrderByDescending(x => x.X).Take(2);
            var topLeft = mostRight.OrderBy(x => x.Y).First();
            return new PointModel(topLeft.X, topLeft.Y);
        }

        private static PointModel GetBottomLeft(PointF[] vertices)
        {
            var mostLeft = vertices.OrderBy(x => x.X).Take(2);
            var bottomLeft = mostLeft.OrderByDescending(x => x.Y).First();
            return new PointModel(bottomLeft.X, bottomLeft.Y);
        }

        private static PointModel GetBottomRight(PointF[] vertices)
        {
            var mostRight = vertices.OrderByDescending(x => x.X).Take(2);
            var bottomRight = mostRight.OrderByDescending(x => x.Y).First();
            return new PointModel(bottomRight.X, bottomRight.Y);
        }
    }
}
