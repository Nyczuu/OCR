namespace OCR.Extensions
{
    public static class StringExtensions
    {
        public static string AddFileNameSuffix(this string fileNameWithExtension, string additionalPart)
        {
            if (!fileNameWithExtension.Contains('.'))
            {
                throw new ArgumentException($"{nameof(fileNameWithExtension)} does not contain extension");
            }

            var extension = fileNameWithExtension.Split('.', StringSplitOptions.RemoveEmptyEntries).Last();

            return fileNameWithExtension.Replace($".{extension}", $"_{additionalPart}.{extension}");
        } 
    }
}
