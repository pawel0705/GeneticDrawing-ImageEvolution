using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

using ImageEvolution.Model.Genetic.Evolution;
using ImageEvolution.Model.Utils;

namespace ImageEvolution
{
    class EvolutionFitness
    {
        private readonly int _imageWidth;
        private readonly int _imageHeight;
        private readonly double _dMax;

        public EvolutionFitness(int imageWidth, int imageHeight)
        {
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;

            _dMax = _imageWidth * _imageHeight * Math.Sqrt(255 * 255 * 3);
        }

        // TODO getpixel jest wolne, zamienc pozniej na lockbity
        public unsafe void CompareImages(Individual individual, Color[,] originalColours)
        {
            double d = 0;

            using (Bitmap bit = new Bitmap(_imageWidth, _imageHeight, PixelFormat.Format24bppRgb))
            {
                using (Graphics graphics = Graphics.FromImage(bit))
                {
                    ImageRenderer.DrawImage(individual, graphics);

                    BitmapData bitmapData = bit.LockBits(new Rectangle(0, 0, _imageWidth, _imageHeight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                    byte bytesPerPixel = 3;

                    byte* scan0 = (byte*)bitmapData.Scan0.ToPointer();
                    int stride = bitmapData.Stride;

                    for (int j = 0; j < _imageHeight; j++)
                    {
                        byte* row = scan0 + (j * stride);

                        for (int i = 0; i < _imageWidth; i++)
                        {
                            int bIndex = i * bytesPerPixel;
                            int gIndex = bIndex + 1;
                            int rIndex = bIndex + 2;

                            byte oRed = originalColours[i, j].R;
                            byte oGreen = originalColours[i, j].G;
                            byte oBlue = originalColours[i, j].B;

                            byte gRed = row[rIndex];
                            byte gGreen = row[gIndex];
                            byte gBlue = row[bIndex];

                            d += Math.Sqrt(Math.Pow(oRed - gRed, 2) + Math.Pow(oGreen - gGreen, 2) + Math.Pow(oBlue - gBlue, 2));
                        }
                    }

                   bit.UnlockBits(bitmapData);
                }
            }

            individual.Adaptation = (_dMax - d) / _dMax * 100;
        }
    }
}
