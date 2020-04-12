using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Genetic.Evolution;
using ImageEvolution.Model.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using static ImageEvolution.ImageOpener;
using System.Windows.Threading;
using System.IO;

namespace ImageEvolution
{
    public partial class EvolutionWindow : UserControl
    {
        private Bitmap _originalBitmap;
        private GenerationCycle _community;

        bool initialized = false;

        Individual drawing;
        Individual gui;

        private System.Drawing.Color[,] sourceColours;/// 

        public EvolutionWindow()
        {
            InitializeComponent();

            _community = new GenerationCycle();
            drawing = new Individual();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
            timer.Start();

            // this.community.IndividualCreated += this.EventIndividualFinished;
        }

        private void timer_Tick(object sender, EventArgs e)
        {

            lock (drawing)
            {
                gui = drawing.CloneIndividual();
            }

            if (gui == null)
                return;

            if (_originalBitmap == null)
                return;


            this.bestGeneticImage.Source = null;

            using (var bit = new Bitmap(_originalBitmap.Width, _originalBitmap.Height))
            {
                Graphics g = Graphics.FromImage(bit);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                ImageRenderer.DrawImage(gui, g);

                bit.Save("test.jpeg", ImageFormat.Jpeg);
            }

            var bitmap = new BitmapImage();
            var stream = File.OpenRead("test.jpeg");

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            stream.Close();
            stream.Dispose();

            this.bestGeneticImage.Source = bitmap;

            this.generation.Content = gui.Generation.ToString();

            this.currentFitness.Content = "Current fitness " + Math.Round(gui.Adaptation, 2).ToString() + "%";
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (initialized == false)
            {
                _community.InitializeEvolution(sourceColours);
                initialized = true;
            }

            this.GenerateButton.IsEnabled = false;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

                while (true)
                {
                    drawing = _community.Generate();
                }

            }).Start();


        }

        public void EventIndividualFinished(object sender, IndividualEventArgs e)
        {
            Dispatcher.Invoke(delegate
            {
                using (var backBuffer = new Bitmap(_originalBitmap.Width, _originalBitmap.Height))
                {
                    using (Graphics g = Graphics.FromImage(backBuffer))
                    {
                        ImageRenderer.DrawImage(drawing, g);

                    }

                }
            });
        }

        private void OriginalImageColours()
        {
            sourceColours = new System.Drawing.Color[_originalBitmap.Width, _originalBitmap.Height];

            for (int i = 0; i < _originalBitmap.Width; i++)
            {
                for (int j = 0; j < _originalBitmap.Height; j++)
                {
                    var color = _originalBitmap.GetPixel(i, j);
                    sourceColours[i, j] = color;
                }
            }
        }

        private void InsertOriginalImage(object sender, RoutedEventArgs e)
        {
            String originalImagePath = ImageOpener.ReadImageFromFile(FileFilter.IMAGE);

            if (originalImagePath != String.Empty)
            {
                _originalBitmap = new Bitmap(originalImagePath);
                this.originalImage.Source = ImageOpener.Bitmap2BitmapImage(_originalBitmap);
                OriginalImageColours();

                AlgorithmSettings.ImageWidth = _originalBitmap.Width;
                AlgorithmSettings.ImageHeight = _originalBitmap.Height;
            }
        }
    }
}
