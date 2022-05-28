// See https://aka.ms/new-console-template for more information
using OCR.Services;

internal class Program
{
    static void Main(string[] args)
    {
        var processor = new ImageProcessor(new DrawingService(), new ShapeService());
        var testData = Directory.GetFiles("TestData").Where(x => !x.Contains("_processed"));

        foreach (var file in testData)
            processor.ProcessImage(file);
    }
}
