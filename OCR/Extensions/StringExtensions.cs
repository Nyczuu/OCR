namespace OCR.Extensions
{
    public static class StringExtensions
    {
        public static string AddFileNameSuffix(this string fileName, string additionalPart, string? newExtension = null)
        {
            var extension = fileName.Split('.', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            var fileNameWithoutExtension = fileName.Replace($".{extension}", string.Empty);
            var fileNameWithSuffix = $"{fileNameWithoutExtension}_{additionalPart}";

            if(extension == null && newExtension == null)
            {
                return fileNameWithSuffix;
            }

            return $"{fileNameWithSuffix}.{newExtension ?? extension}";
        } 
    }
}
