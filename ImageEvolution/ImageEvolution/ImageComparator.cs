using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEvolution
{
    class ImageComparator
    {
        private int _imageWidth;
        private int _imageHeight;
        private double _dMax;

        public ImageComparator(int imageWidth, int imageHeight)
        {
            _imageWidth = imageWidth;
            _imageHeight = imageHeight;

            _dMax = _imageWidth * _imageHeight * Math.Sqrt(255 * 255 * 3);
        }

        // TODO getpixel jest wolne, zamienc pozniej na lockbity
        public double CompareImages(Bitmap originalImage, Bitmap geneticImage)
        {
            double d = 0;
            for(int i = 0; i < _imageWidth; i++)
            {
                for(int j = 0; j < _imageHeight; j++)
                {
                    int oRed = originalImage.GetPixel(i, j).R;
                    int oGreen = originalImage.GetPixel(i, j).G;
                    int oBlue = originalImage.GetPixel(i, j).B;

                    int gRed = geneticImage.GetPixel(i, j).R;
                    int gGreen = geneticImage.GetPixel(i, j).G;
                    int gBlue = geneticImage.GetPixel(i, j).B;

                    d+= Math.Sqrt(Math.Pow(oRed - gRed, 2) + Math.Pow(oGreen - gGreen, 2) + Math.Pow(oBlue - gBlue, 2));
                }
            }

            return (_dMax - d) / _dMax;
        }
    }
}
