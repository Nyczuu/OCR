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
    }
}
