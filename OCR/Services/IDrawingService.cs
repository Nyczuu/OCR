using Emgu.CV;
using Emgu.CV.Structure;

namespace OCR.Services
{
    public interface IDrawingService
    {
        void DrawLabel(Mat img, string label);
        void DrawFrame(Mat img);
        void DrawRectangle(Mat img, RotatedRect rectangle);
        void DrawArrow(Mat img, Arrow arrow);
    }
}
