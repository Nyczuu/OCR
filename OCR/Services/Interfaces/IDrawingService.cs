using Emgu.CV;
using Emgu.CV.Structure;

namespace OCR.Services.Interfaces
{
    public interface IDrawingService
    {
        void DrawRectangles(Mat img, ICollection<RotatedRect> rectangleList);
        void DrawArrows(Mat img, ICollection<Arrow> arrowList);
    }
}
