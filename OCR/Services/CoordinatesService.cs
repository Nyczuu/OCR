using OCR.Models;
using OCR.Services.Interfaces;

namespace OCR.Services
{
    public class CoordinatesService : ICoordinatesService
    {
        public ProcessedImageModel ExtractCoordtinates(ProcessedImage image)
        {
            var result = new ProcessedImageModel();

            foreach (var rectangle in image.Rectangles)
            {
                var center = new PointModel(rectangle.Center.X, rectangle.Center.Y);
                PointModel topLeft = null;
                PointModel topRight = null;
                PointModel bottomLeft = null;
                PointModel bottomRight = null;
                result.RectangleModels.Add(new RectangleModel(center, topLeft, topRight, bottomLeft, bottomRight));
            }

            return result;
        }
    }
}
