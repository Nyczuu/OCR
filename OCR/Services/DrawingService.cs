﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace OCR.Services
{
    internal class DrawingService : IDrawingService
    {
        public void DrawLabel(Mat img, string label)
             => CvInvoke.PutText(img, label, new Point(20, 20), FontFace.HersheyDuplex, 0.5, new MCvScalar(120, 120, 120));

        public void DrawFrame(Mat img)
             => CvInvoke.Rectangle(img, new Rectangle(Point.Empty, new Size(img.Width - 1, img.Height - 1)), new MCvScalar(120, 120, 120));

        public void DrawRectangle(Mat img, RotatedRect rectangle)
            => CvInvoke.Polylines(img, Array.ConvertAll(rectangle.GetVertices(), Point.Round), true, new Bgr(Color.DarkOrange).MCvScalar, 2);

        public void DrawArrow(Mat img, Arrow arrow)
        {
            foreach (var line in arrow.Tail)
                CvInvoke.Line(img, line.P1, line.P2, new Bgr(Color.Blue).MCvScalar, 2);

            foreach (var line in arrow.Head)
                CvInvoke.Line(img, line.P1, line.P2, new Bgr(Color.Blue).MCvScalar, 2);
        }
    }
}
