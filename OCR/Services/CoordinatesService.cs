using Emgu.CV.Structure;
using OCR.Models;
using OCR.Services.Interfaces;
using System.Drawing;

namespace OCR.Services
{
    public class CoordinatesService : ICoordinatesService
    {
        public ProcessedImageModel ExtractCoordtinates(ProcessedImage image)
        {
            var result = new ProcessedImageModel();

            foreach (var rectangle in image.Rectangles)
            {
                result.Rectangles.Add(RectangleModel.FromRotatedRect(rectangle));
            }

            foreach(var arrow in image.Arrows)
            {
                result.Relations.Add(GetClosestRectangles(arrow, result.Rectangles));
            }

            return result;
        }

        private RelationModel GetClosestRectangles(Arrow arrow, ICollection<RectangleModel> rectangles)
        {
            var closestsBeginId = rectangles.First().Id;
            var closestsBeginDist = double.MaxValue;
            var closestEndId = rectangles.Last().Id;
            var closestsEndDist = double.MaxValue;

            foreach (var rectangle in rectangles)
            {
                var hookingPoints = rectangle.HookingPoints();
                var lowestEndDist = hookingPoints.Min(p => PointHelper.MeasureDistance(p, arrow.TailEnd));

                if (lowestEndDist < closestsEndDist)
                {
                    closestsEndDist = lowestEndDist;
                    closestEndId = rectangle.Id;
                }

                var lowestBeginDist = hookingPoints.Min(p => PointHelper.MeasureDistance(p, arrow.TailBegin));

                if (lowestBeginDist < closestsBeginDist)
                {
                    closestsBeginDist = lowestBeginDist;
                    closestsBeginId = rectangle.Id;
                }
            }

            return new RelationModel(closestEndId, closestsBeginId);
        }
    }
}
