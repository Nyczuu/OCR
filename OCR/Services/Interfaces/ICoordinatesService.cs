using OCR.Models;

namespace OCR.Services.Interfaces
{
    public interface ICoordinatesService
    {
        ProcessedImageModel ExtractCoordtinates(ProcessedImage image);
    }
}
