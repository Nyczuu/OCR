namespace OCR.Extensions
{
    public static class FloatExtensions
    {
        public static int RoundToFives(this float value)
        {
            var result = (int)Math.Round(value, 0);
            var modulo = result % 5;

            if(modulo == 0)
                return result;

            return modulo < 3 ? result - modulo : result + 5 - modulo;
        }
    }
}
