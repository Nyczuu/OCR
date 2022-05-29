// See https://aka.ms/new-console-template for more information
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OCR;
using OCR.Models;
using OCR.Services.Interfaces;
using System.Drawing;

public class ImageProcessor
{
    private IDrawingService _drawingService;
    private IShapeService _shapeService;

    public ImageProcessor(IDrawingService drawingService, IShapeService shapeService)
    {
        _drawingService = drawingService;
        _shapeService = shapeService;
    }

    public ProcessedImage ProcessImage(string imgPath)
    {
        Console.WriteLine($"Processing: `{imgPath}`");

        using var img = CvInvoke.Imread(imgPath, ImreadModes.AnyColor);
        using UMat gray = new();
        using UMat cannyEdges = new();
        using Mat rectangleImage = new(img.Size, DepthType.Cv8U, 3);
        using Mat arrowImage = new(img.Size, DepthType.Cv8U, 3);
        rectangleImage.SetTo(new MCvScalar(0));
        arrowImage.SetTo(new MCvScalar(0));

        AlignColors(img, gray);
        BlurImage(gray, imgPath);
        FindEdges(gray, cannyEdges, imgPath);

        using var contours = new VectorOfVectorOfPoint();
        CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

        var rectangles = HandleRectangles(rectangleImage, contours);
        var arrows = HandleArrows(arrowImage, contours);
        HandleResult(imgPath, img, rectangleImage, arrowImage);

        return new ProcessedImage(rectangles, arrows);
    }

    private void FindEdges(UMat input, UMat output, string imgPath)
    {
        var canyPath = imgPath.Replace(".png", "_canny.png");
        CvInvoke.Canny(input, output, threshold1: 180.0, threshold2: 120.0);
        output.Save(canyPath);
        Console.WriteLine($"Created Canny edges image: `{canyPath}`");
    }

    private void BlurImage(UMat img, string imgPath)
    {
        var bluredPath = imgPath.Replace(".png", "_blured.png");
        CvInvoke.GaussianBlur(img, img, new Size(3, 3), 1);
        img.Save(bluredPath);
        Console.WriteLine($"Created blured image: `{bluredPath}`");
    }

    private void AlignColors(Mat img, UMat gray)
    {
        if (img.NumberOfChannels < 3)
        {
            Console.WriteLine("Detected gray scale image.");
            CvInvoke.CvtColor(img, img, ColorConversion.Gray2Bgr);

        }
        else
        {
            Console.WriteLine("Detected color image.");
        }

        CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);
    }

    private void HandleResult(string imgPath, Mat img, Mat rectangleImage, Mat arrowImage)
    {
        var processedPath = imgPath.Replace(".png", "_processed.png");
        using var result = new Mat();
        CvInvoke.VConcat(new Mat[] { img, rectangleImage, arrowImage }, result);
        result.Save(processedPath);
        Console.WriteLine($"Created processed image: `{processedPath}`");
    }

    private ICollection<RotatedRect> HandleRectangles(Mat img, VectorOfVectorOfPoint contours)
    {
        var rectangleList = _shapeService.FindRectangles(contours);

        foreach (RotatedRect rectangle in rectangleList)
            _drawingService.DrawRectangle(img, rectangle);

        _drawingService.DrawFrame(img);
        _drawingService.DrawLabel(img, "Rectangles");

        Console.WriteLine($"Found {rectangleList.Count} rectangles.");

        return rectangleList;
    }

    private ICollection<Arrow> HandleArrows(Mat img, VectorOfVectorOfPoint contours)
    {
        var arrowList = _shapeService.FindArrows(contours);

        foreach (var arrow in arrowList)
        {
            _drawingService.DrawArrow(img, arrow);
        }

        _drawingService.DrawFrame(img);
        _drawingService.DrawLabel(img, "Arrows");

        Console.WriteLine($"Found {arrowList.Count} arrows.");

        return arrowList;
    }
}

