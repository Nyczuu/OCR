using OCR.Models;

namespace OCR.Services.Interfaces
{
    public interface ICoordinatesService
    {
        void ExtractCoordtinates(ProcessedImage image);
    }
}
