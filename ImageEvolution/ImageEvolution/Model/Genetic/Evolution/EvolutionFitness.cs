using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Utils;

namespace ImageEvolution
{
    class EvolutionFitness
    {
        private int _imageWidth;
        private int _imageHeight;
        private double _dMax;

        public EvolutionFitness(int imageWidth, int imageHeight)
        {
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;

            _dMax = _imageWidth * _imageHeight * Math.Sqrt(255 * 255 * 3);
        }

        // TODO getpixel jest wolne, zamienc pozniej na lockbity
        public double CompareImages(ImageDNA individual, Color[,] originalColours)
        {
            double d = 0;

            using (var bit = new Bitmap(_imageWidth, _imageHeight, PixelFormat.Format24bppRgb))
            {
                using (Graphics graphics = Graphics.FromImage(bit))
                {
                    ImageRenderer.DrawImage(individual, graphics);

                    BitmapData bitmapData = bit.LockBits(new Rectangle(0, 0, _imageWidth, _imageHeight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


                    for (int i = 0; i < _imageWidth; i++)
                    {
                        for (int j = 0; j < _imageHeight; j++)
                        {
                            byte oRed = originalColours[i, j].R;
                            byte oGreen = originalColours[i,j].G;
                            byte oBlue = originalColours[i,j].B;

                            byte gRed = GetPixel(bitmapData, i, j).R;
                            byte gGreen = GetPixel(bitmapData, i, j).G;
                            byte gBlue = GetPixel(bitmapData, i, j).B;

                            d += Math.Sqrt(Math.Pow(oRed - gRed, 2) + Math.Pow(oGreen - gGreen, 2) + Math.Pow(oBlue - gBlue, 2));
                        }
                    }
                }
            }

            return (_dMax - d) / _dMax;
        }

        private static unsafe Color GetPixel(BitmapData bmd, int x, int y)
        {
            byte* p = (byte*)bmd.Scan0 + y * bmd.Stride + 3 * x;
            return Color.FromArgb(p[2], p[1], p[0]);
        }
    }
}
