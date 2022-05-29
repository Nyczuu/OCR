namespace OCR.Models
{
    public class ProcessedImageModel
    {
        public ICollection<RectangleModel> Rectangles { get; } = new List<RectangleModel>();
        public ICollection<RelationModel> Relations { get; } = new List<RelationModel>();
    }
}