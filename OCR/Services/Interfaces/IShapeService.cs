using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace OCR.Services.Interfaces
{
    public interface IShapeService
    {
        ICollection<RotatedRect> FindRectangles(VectorOfVectorOfPoint contours);
        ICollection<Arrow> FindArrows(VectorOfVectorOfPoint contours);
    }
}
