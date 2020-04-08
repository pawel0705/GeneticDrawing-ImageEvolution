using System;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows;

namespace ImageEvolution
{
    static class ImageOpener
    {
        public enum FileFilter { 
            VOID = 0,
            IMAGE = 1
        }

        public static string ImageFilter = "gif files (*.gif)|*.gif|bmp files (*.bmp)|*.bmp|jpg files (*.jpg)|*.jpg|jpeg files (*.jpeg)|*.jpeg|All files (*.*)|*.*";

        public static string ReadImageFromFile(FileFilter fileFilter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            switch (fileFilter)
            {
                case FileFilter.IMAGE:
                    openFileDialog.Filter = ImageFilter;
                    break;
            }

            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true) // if the file selection window opens
            {
                return openFileDialog.InitialDirectory + openFileDialog.FileName; // getting the path to the file
            }

            return String.Empty;
        }

        public static BitmapSource Bitmap2BitmapImage(Bitmap bitmap)
        {

            BitmapSource i = Imaging.CreateBitmapSourceFromHBitmap(
                           bitmap.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            return i;
        }
    }
}