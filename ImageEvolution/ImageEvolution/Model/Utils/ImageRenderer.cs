using ImageEvolution.Model.Genetic.Evolution;
using System;
using System.Collections.Generic;
using ImageEvolution.Model.Genetic.Chromosome;
using System.Drawing;

using System.Text;
using ImageEvolution.Model.Genetic.DNA;

namespace ImageEvolution.Model.Utils
{
    public static class ImageRenderer
    {
        public static void DrawImage(Individual image, Graphics graphics)
        {
            graphics.Clear(Color.White);

            var maxIterator = image.SquareShapes.Count + 
                image.ElipseShapes.Count + 
                image.TriangleShapes.Count +
                image.PentagonShapes.Count;

            for(int i = 0; i < maxIterator; i++)
            {

                if(image.SquareShapes.Count > i)
                {
                    DrawFigure(image.SquareShapes[i], graphics);
                }

                if(image.ElipseShapes.Count > i)
                {
                    DrawElipse(image.ElipseShapes[i], graphics);
                }

                if(image.TriangleShapes.Count > i)
                {
                    DrawFigure(image.TriangleShapes[i], graphics);
                }

                if (image.PentagonShapes.Count > i)
                {
                    DrawFigure(image.PentagonShapes[i], graphics);
                }

                if (image.TriangleShapes.Count < i &&
                    image.ElipseShapes.Count < i &&
                    image.SquareShapes.Count < i &&
                    image.PentagonShapes.Count < i)
                {
                    break;
                }
            }
        }

        private static void DrawElipse(ShapeChromosome shape, Graphics graphics)
        {
            using (Brush brush = GetSolidColour(shape.ColourShape))
            {
                Point[] points = GetGdiPoints(shape.PositionsShape);
                graphics.FillEllipse(brush, points[0].X, points[0].Y, points[1].X, points[1].Y);
            }

        }

        private static void DrawFigure(ShapeChromosome shape, Graphics graphics)
        {
            using (Brush brush = GetSolidColour(shape.ColourShape))
            {
                Point[] points = GetGdiPoints(shape.PositionsShape);
                graphics.FillPolygon(brush, points);
            }
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
