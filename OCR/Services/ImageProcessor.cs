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
        using var img = CvInvoke.Imread(imgPath, ImreadModes.AnyColor);
        using UMat gray = new();
        using UMat cannyEdges = new();
        using Mat rectangleImage = new(img.Size, DepthType.Cv8U, 3);
        using Mat arrowImage = new(img.Size, DepthType.Cv8U, 3);
        rectangleImage.SetTo(new MCvScalar(0));
        arrowImage.SetTo(new MCvScalar(0));

        if (img.NumberOfChannels < 3)
            CvInvoke.CvtColor(img, img, ColorConversion.Gray2Bgr);

        CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);
        CvInvoke.GaussianBlur(gray, gray, new Size(3, 3), 1);
        CvInvoke.Canny(gray, cannyEdges, threshold1: 180.0, threshold2: 120.0);

        using var contours = new VectorOfVectorOfPoint();
        CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

        var rectangles = HandleRectangles(rectangleImage, contours);
        var arrows = HandleArrows(arrowImage, contours);
        HandleResult(imgPath, img, rectangleImage, arrowImage);

        return new ProcessedImage(rectangles, arrows);
    }

    private void HandleResult(string imgPath, Mat img, Mat rectangleImage, Mat arrowImage)
    {
        using var result = new Mat();
        CvInvoke.VConcat(new Mat[] { img, rectangleImage, arrowImage }, result);
        result.Save(imgPath.Replace(".png", "_processed.png"));
    }

    private ICollection<RotatedRect> HandleRectangles(Mat img, VectorOfVectorOfPoint contours)
    {
        var boxList = _shapeService.FindBoxes(contours);

        foreach (RotatedRect box in boxList)
            _drawingService.DrawRectangle(img, box);

        _drawingService.DrawFrame(img);
        _drawingService.DrawLabel(img, "Rectangles");

        return boxList;
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

        return arrowList;
    }
}

