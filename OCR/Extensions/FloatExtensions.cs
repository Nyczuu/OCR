namespace OCR.Extensions
{
    public static class FloatExtensions
    {
        public static int RoundToFives(this float value) => ((int)Math.Round(value, 0)).RoundToFives();
    }
}
