// See https://aka.ms/new-console-template for more information
using OCR.Extensions;
using OCR.Services;

internal class Program
{
    static void Main(string[] args)
    {
        var path = "TestData";
        if (args.Any())
        {
            path = args.First();
        }

        Console.WriteLine($"Selected folder: `{path}`.");
        var processor = new ImageProcessor(new DrawingService(), new ShapeService());
        var coordinatesService = new CoordinatesService();
        var testData = Directory.GetFiles(path);
        var runDirectory = Directory.CreateDirectory(Path.Combine(path, Guid.NewGuid().ToString()));

        foreach (var file in testData)
        {
            var fileDir = Directory.CreateDirectory(Path.Combine(runDirectory.FullName, Path.GetFileNameWithoutExtension(file)));
            var workingCopyPath = Path.Combine(fileDir.FullName, Path.GetFileName(file).OverrideExtension("original"));
            File.Copy(file, workingCopyPath);

            var result = processor.ProcessImage(workingCopyPath);
            coordinatesService.ExtractCoordtinates(result);
            Console.WriteLine();
        }
    }
}
