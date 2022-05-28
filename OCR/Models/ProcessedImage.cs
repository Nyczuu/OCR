using Emgu.CV.Structure;

namespace OCR.Models
{
    public class ProcessedImage
    {
        public ProcessedImage(ICollection<RotatedRect> rectangles, ICollection<Arrow> arrows)
        {
            Rectangles = rectangles;
            Arrows = arrows;
        }

        public ICollection<RotatedRect> Rectangles { get; }
        public ICollection<Arrow> Arrows { get; }
    }
}
