// See https://aka.ms/new-console-template for more information
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OCR;
using System.Drawing;

Console.WriteLine("Hello, World!");
using var img = CvInvoke.Imread("test1.jpeg", ImreadModes.AnyColor);
var processor = new ImageProcessor();
var processedImg = processor.ProcessImage(img);
processedImg.Save("processed.png");


public class ImageProcessor
{
    public Mat ProcessImage(Mat img)
    {
        using UMat gray = new();
        using UMat cannyEdges = new();
        using Mat rectangleImage = new(img.Size, DepthType.Cv8U, 3); //image to draw and rectangles on
        using Mat lineImage = new(img.Size, DepthType.Cv8U, 3); //image to draw lines on
        using Mat arrowImage = new(img.Size, DepthType.Cv8U, 3);

        if (img.NumberOfChannels < 3)
        {
            CvInvoke.CvtColor(img, img, ColorConversion.Gray2Bgr);
        }

        CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);

        CvInvoke.GaussianBlur(gray, gray, new Size(3, 3), 1);
        gray.Save("blured.png");

        double cannyThreshold = 180.0;
        double cannyThresholdLinking = 120.0;
        CvInvoke.Canny(gray, cannyEdges, cannyThreshold, cannyThresholdLinking);
        LineSegment2D[] lines = CvInvoke.HoughLinesP(cannyEdges, 1, Math.PI / 45.0, 10, 10, 20);
        cannyEdges.Save("canny.png");

        List<Arrow> headList = new List<Arrow>();
        List<Arrow> tailList = new List<Arrow>();
        List<RotatedRect> boxList = new List<RotatedRect>();
        using var contours = new VectorOfVectorOfPoint();
        CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

        for (var i = 0; i < contours.Size; i++)
        {
            using var contour = contours[i];
            using var approxContour = new VectorOfPoint();
            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);

            if (approxContour.Size == 4) //The contour has 4 vertices.
            {
                #region determine if all the angles in the contour are within [80, 100] degree
                bool isRectangle = true;
                LineSegment2D[] edges = PointCollection.PolyLine(approxContour.ToArray(), true);

                for (int j = 0; j < edges.Length; j++)
                {
                    double angle = Math.Abs(edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                    if (angle < 80 || angle > 100)
                    {
                        isRectangle = false;
                        break;
                    }
                }

                #endregion

                if (isRectangle)
                {
                    boxList.Add(CvInvoke.MinAreaRect(approxContour));
                }
                else
                {
                    headList.Add(Arrow.FromLines(edges));
                }
            }
            else if (approxContour.Size == 2)
            {
                LineSegment2D[] edges = PointCollection.PolyLine(approxContour.ToArray(), true);
                tailList.Add(Arrow.FromLines(edges));
            }
        }

        rectangleImage.SetTo(new MCvScalar(0));

        foreach (RotatedRect box in boxList)
        {
            CvInvoke.Polylines(rectangleImage, Array.ConvertAll(box.GetVertices(), Point.Round), true, new Bgr(Color.DarkOrange).MCvScalar, 2);
        }

        AddFrame(rectangleImage);
        AddLabel(rectangleImage, "Rectangles");


        lineImage.SetTo(new MCvScalar(0));
        foreach (var arrow in headList)
            foreach (var line in arrow.Lines)
                CvInvoke.Line(arrowImage, line.P1, line.P2, new Bgr(Color.Blue).MCvScalar, 2);

        foreach (var arrow in tailList)
            foreach (var line in arrow.Lines)
                CvInvoke.Line(arrowImage, line.P1, line.P2, new Bgr(Color.Blue).MCvScalar, 2);

        AddFrame(arrowImage);
        AddLabel(arrowImage, "Arrows");

        lineImage.SetTo(new MCvScalar(0));
        foreach (LineSegment2D line in lines)
            CvInvoke.Line(lineImage, line.P1, line.P2, new Bgr(Color.Green).MCvScalar, 2);

        AddFrame(lineImage);
        AddLabel(lineImage, "Lines");


        Mat result = new Mat();
        CvInvoke.VConcat(new Mat[] { img, rectangleImage, arrowImage, lineImage }, result);
        return result;

        static void AddLabel(Mat img, string label)
            => CvInvoke.PutText(img, label, new Point(20, 20), FontFace.HersheyDuplex, 0.5, new MCvScalar(120, 120, 120));

        static void AddFrame(Mat img) 
            => CvInvoke.Rectangle(img, new Rectangle(Point.Empty, new Size(img.Width - 1, img.Height - 1)), new MCvScalar(120, 120, 120));
    }
}

