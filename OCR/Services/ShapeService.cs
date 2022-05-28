using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace OCR.Services
{
    public class ShapeService : IShapeService
    {
        private const int MinArea = 10;

        public ICollection<Arrow> FindArrows(VectorOfVectorOfPoint contours)
        {
            List<LineSegment2D[]> potentialTailList = new List<LineSegment2D[]>();
            List<Arrow> arrowList = new List<Arrow>();

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
                if (CvInvoke.ContourArea(contour) > MinArea)
                {
                    if (approxContour.Size > 2)
                    {
                        var edges = PointCollection.PolyLine(approxContour.ToArray(), true);
                        
                        if (!IsRectangle(edges))
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

            return arrowList;
        }

        public ICollection<RotatedRect> FindBoxes(VectorOfVectorOfPoint contours)
        {
            List<RotatedRect> rectangles = new List<RotatedRect>();

            for (var i = 0; i < contours.Size; i++)
            {
                using var contour = contours[i];
                using var approxContour = new VectorOfPoint();

                CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
                if (CvInvoke.ContourArea(contour) > MinArea)
                {
                    if (approxContour.Size == 4)
                    {
                        var edges = PointCollection.PolyLine(approxContour.ToArray(), true);
                        
                        if (IsRectangle(edges))
                        {
                            rectangles.Add(CvInvoke.MinAreaRect(approxContour));
                        }
                    }
                }
            }

            return rectangles;
        }

        private static bool IsRectangle(LineSegment2D[] edges)
        {
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

            return isRectangle;
        }
    }
}
