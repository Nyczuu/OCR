// See https://aka.ms/new-console-template for more information
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
        var testData = Directory.GetFiles(path).Where(x => !x.Contains("_processed"));

        foreach (var file in testData)
        {
            var result = processor.ProcessImage(file);
            coordinatesService.ExtractCoordtinates(result);
            Console.WriteLine();
        }
    }
}
