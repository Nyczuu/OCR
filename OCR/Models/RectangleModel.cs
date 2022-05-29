using Emgu.CV.Structure;
using System.Drawing;

namespace OCR.Models
{
    public class RectangleModel
    {
        private ICollection<PointF> _hookingPoints;

        public RectangleModel(PointF center, PointF topLeft, PointF topRight, PointF bottomLeft, PointF bottomRight)
        {
            Id = Guid.NewGuid();
            Center = center;
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;

            CenterTop = PointHelper.PointBetween(topLeft, topRight);
            CenterBottom = PointHelper.PointBetween(bottomLeft, bottomRight);
            CenterLeft = PointHelper.PointBetween(topLeft, bottomLeft);
            CenterRight = PointHelper.PointBetween(topRight, bottomRight);

            _hookingPoints = new List<PointF> { TopLeft, CenterTop, TopRight, CenterLeft, CenterRight, BottomLeft, CenterBottom, BottomRight };
        }

        public Guid Id { get; }
        public PointF Center { get; }
        public PointF CenterTop { get; }
        public PointF CenterBottom { get; }
        public PointF CenterLeft { get; }
        public PointF CenterRight { get; }
        public PointF TopLeft { get; }
        public PointF TopRight { get; }
        public PointF BottomLeft { get; }
        public PointF BottomRight { get; }

        public static RectangleModel FromRotatedRect(RotatedRect rotatedRect)
        {
            var center = new PointF(rotatedRect.Center.X, rotatedRect.Center.Y);
            var vertices = rotatedRect.GetVertices();

            return new RectangleModel(center, GetTopLeft(vertices), GetTopRight(vertices), GetBottomLeft(vertices), GetBottomRight(vertices));
        }

        public ICollection<PointF> HookingPoints() => _hookingPoints;

        private static PointF GetTopLeft(PointF[] vertices)
        {
            var mostLeft = vertices.OrderBy(x => x.X).Take(2);
            var topLeft = mostLeft.OrderBy(x => x.Y).First();
            return new PointF(topLeft.X, topLeft.Y);
        }

        private static PointF GetTopRight(PointF[] vertices)
        {
            var mostRight = vertices.OrderByDescending(x => x.X).Take(2);
            var topLeft = mostRight.OrderBy(x => x.Y).First();
            return new PointF(topLeft.X, topLeft.Y);
        }

        private static PointF GetBottomLeft(PointF[] vertices)
        {
            var mostLeft = vertices.OrderBy(x => x.X).Take(2);
            var bottomLeft = mostLeft.OrderByDescending(x => x.Y).First();
            return new PointF(bottomLeft.X, bottomLeft.Y);
        }

        private static PointF GetBottomRight(PointF[] vertices)
        {
            var mostRight = vertices.OrderByDescending(x => x.X).Take(2);
            var bottomRight = mostRight.OrderByDescending(x => x.Y).First();
            return new PointF(bottomRight.X, bottomRight.Y);
        }
    }
}
