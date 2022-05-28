// See https://aka.ms/new-console-template for more information
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OCR;
using System.Drawing;

public class ImageProcessor
{
    public Mat ProcessImage(Mat img)
    {
        using UMat gray = new();
        using UMat cannyEdges = new();
        using Mat rectangleImage = new(img.Size, DepthType.Cv8U, 3);
        using Mat arrowImage = new(img.Size, DepthType.Cv8U, 3);
        rectangleImage.SetTo(new MCvScalar(0));
        arrowImage.SetTo(new MCvScalar(0));

        if (img.NumberOfChannels < 3)
        {
            CvInvoke.CvtColor(img, img, ColorConversion.Gray2Bgr);
        }

        CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);
        CvInvoke.GaussianBlur(gray, gray, new Size(3, 3), 1);
        CvInvoke.Canny(gray, cannyEdges, threshold1: 180.0, threshold2: 120.0);

        List<LineSegment2D[]> potentialTailList = new List<LineSegment2D[]>();
        List<RotatedRect> boxList = new List<RotatedRect>();
        List<Arrow> arrowList = new List<Arrow>();

        using var contours = new VectorOfVectorOfPoint();
        CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

        for (var i = 0; i < contours.Size; i++)
        {
            using var contour = contours[i];
            using var approxContour = new VectorOfPoint();
            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);

            if (approxContour.Size == 2)
            {
                var line = PointCollection.PolyLine(approxContour.ToArray(), true);
                if (line.Any(x => x.Length >= 30))
                {
                    potentialTailList.Add(line);
                }
            }
        }

        for (var i = 0; i < contours.Size; i++)
        {
            using var contour = contours[i];
            using var approxContour = new VectorOfPoint();

            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
            var area = CvInvoke.ContourArea(contour);
            if (area > 10)
            {
                if (approxContour.Size > 2)
                {
                    LineSegment2D[] edges = PointCollection.PolyLine(approxContour.ToArray(), true);
                    bool isRectangle = true;

                    for (int j = 0; j < edges.Length; j++)
                    {
                        double angle = Math.Abs(edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                        if (angle < 88 || angle > 92)
                        {
                            isRectangle = false;
                            break;
                        }
                    }

                    if (isRectangle)
                    {
                        boxList.Add(CvInvoke.MinAreaRect(approxContour));
                    }
                    else
                    {
                        foreach (var potentialTail in potentialTailList)
                        {
                            var distanceP1 = CvInvoke.PointPolygonTest(contour, potentialTail.First().P1, true);
                            var distanceP2 = CvInvoke.PointPolygonTest(contour, potentialTail.First().P2, true);

                            if (Math.Min(Math.Abs(distanceP1), Math.Abs(distanceP2)) <= 2)
                            {
                                arrowList.Add(new Arrow(edges, potentialTail));
                            }
                        }
                    }
                }
            }
        }

        foreach (RotatedRect box in boxList)
        {
            CvInvoke.Polylines(rectangleImage, Array.ConvertAll(box.GetVertices(), Point.Round), true, new Bgr(Color.DarkOrange).MCvScalar, 2);
        }

        AddFrame(rectangleImage);
        AddLabel(rectangleImage, "Rectangles");

        foreach (var arrow in arrowList)
        {
            foreach (var line in arrow.Tail)
                CvInvoke.Line(arrowImage, line.P1, line.P2, new Bgr(Color.Blue).MCvScalar, 2);

            foreach (var line in arrow.Head)
                CvInvoke.Line(arrowImage, line.P1, line.P2, new Bgr(Color.Blue).MCvScalar, 2);
        }

        AddFrame(arrowImage);
        AddLabel(arrowImage, "Arrows");

        Mat result = new Mat();
        CvInvoke.VConcat(new Mat[] { img, rectangleImage, arrowImage }, result);
        return result;

        static void AddLabel(Mat img, string label)
            => CvInvoke.PutText(img, label, new Point(20, 20), FontFace.HersheyDuplex, 0.5, new MCvScalar(120, 120, 120));

        static void AddFrame(Mat img)
            => CvInvoke.Rectangle(img, new Rectangle(Point.Empty, new Size(img.Width - 1, img.Height - 1)), new MCvScalar(120, 120, 120));
    }
}

