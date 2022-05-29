namespace OCR.Models
{
    public class ProcessedImageModel
    {
        public ICollection<RectangleModel> RectangleModels { get; } = new List<RectangleModel>();
    }
}