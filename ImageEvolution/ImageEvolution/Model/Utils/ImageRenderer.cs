using ImageEvolution.Model.Genetic.DNA;
using System;
using System.Collections.Generic;
using System.Drawing;

using System.Text;

namespace ImageEvolution.Model.Utils
{
    public static class ImageRenderer
    {
        public static void DrawImage(ImageDNA image, Graphics graphics)
        {
            graphics.Clear(Color.White);

            foreach(var shape in image.Shapes)
            {
                DrawShape(shape, graphics);
            }
        }

        private static void DrawShape(ShapeDNA shape, Graphics graphics)
        {
            using Brush brush = GetSolidColour(shape.ColourShape);
            Point[] points = GetGdiPoints(shape.PositionsShape);
            graphics.FillPolygon(brush, points);
        }

        private static Point[] GetGdiPoints(IList<PositionDNA> points)
        {
            Point[] pts = new Point[points.Count];
            int i = 0;
            foreach (var pt in points)
            {
                pts[i++] = new Point(pt.PositionX, pt.PositionY);
            }
            return pts;
        }

        private static Brush GetSolidColour(ColourDNA colour)
        {
            return new SolidBrush(Color.FromArgb(colour.AlphaColour, colour.RedColour, colour.GreenColour, colour.BlueColour));
        }
    }
}
