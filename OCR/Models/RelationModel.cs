namespace OCR.Models
{
    public class RelationModel
    {
        public RelationModel(Guid from, Guid to)
        {
            From = from;
            To = to;
        }

        public Guid From { get; }
        public Guid To { get; }
    }
}
