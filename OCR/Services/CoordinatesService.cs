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
                result.RectangleModels.Add(RectangleModel.FromRotatedRect(rectangle));
            }

            return result;
        }
    }
}
