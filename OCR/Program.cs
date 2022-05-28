// See https://aka.ms/new-console-template for more information
using Emgu.CV;
using Emgu.CV.CvEnum;

var processor = new ImageProcessor();
var testData = Directory.GetFiles("TestData").Where(x => !x.Contains("_processed") && x.Contains("FourItems_Connected"));

foreach (var file in testData)
{
    using var img = CvInvoke.Imread(file, ImreadModes.AnyColor);
    var processedImg = processor.ProcessImage(img);
    processedImg.Save(file.Replace(".png", "_processed.png"));
}
