namespace OCR.Extensions
{
    public static class IntegerExtensions
    {
        public static int RoundToFives(this int value)
        {
            var modulo = value % 5;

            if (modulo == 0)
                return value;

            return modulo < 3 ? value - modulo : value + 5 - modulo;
        }
    }
}
