namespace OCR.Models
{
    internal class PointModel
    {
        public PointModel(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }
    }
}
