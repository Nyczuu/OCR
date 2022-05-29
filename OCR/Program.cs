// See https://aka.ms/new-console-template for more information
using OCR.Extensions;
using OCR.Services;

internal class Program
{
    private const string DefaultInputPath = "TestData";

    static void Main(string[] args)
    {
        var processor = new ImageProcessor(new DrawingService(), new ShapeService());
        var coordinatesService = new CoordinatesService();

        var inputPath = GetInputPath(args);
        var runDirectory = Directory.CreateDirectory(Path.Combine(inputPath, Guid.NewGuid().ToString()));

        foreach (var file in Directory.GetFiles(inputPath))
        {
            var localCopyPath = CreateLocalCopy(runDirectory, file);

            var result = processor.ProcessImage(localCopyPath);

            coordinatesService.ExtractCoordtinates(result);

            Console.WriteLine();
        }
    }

    private static string CreateLocalCopy(DirectoryInfo runDirectory, string file)
    {
        var fileDir = Directory.CreateDirectory(Path.Combine(runDirectory.FullName, Path.GetFileNameWithoutExtension(file)));
        var workingCopyPath = Path.Combine(fileDir.FullName, Path.GetFileName(file));
        File.Copy(file, workingCopyPath);
        return workingCopyPath;
    }

    private static string GetInputPath(string[] args)
    {
        var path = args.Any() ? args.First() : DefaultInputPath;

        Console.WriteLine($"Selected folder: `{path}`.");

        return path;
    }
}
