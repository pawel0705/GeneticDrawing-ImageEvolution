using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System;
using Microsoft.Win32;

namespace ImageEvolution
{
    static class ImageOpener
    {
        public static string ReadImageFromFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true) // if the file selection window opens
            {
                return openFileDialog.InitialDirectory + openFileDialog.FileName; // getting the path to the file
            }

            return String.Empty;
        }
    }
}