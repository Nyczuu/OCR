using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace OCR.Services
{
    public interface IShapeService
    {
        ICollection<RotatedRect> FindBoxes(VectorOfVectorOfPoint contours);
        ICollection<Arrow> FindArrows(VectorOfVectorOfPoint contours);
    }
}
