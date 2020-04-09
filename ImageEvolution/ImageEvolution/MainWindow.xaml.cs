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

namespace ImageEvolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap originalBitmap;
        Bitmap geneticBitmap;

        EvolutionFitness evolutionFitness;
        GenerationCycle community;
        bool initialized = false;
        Individual drawing;
        Individual gui;

        private System.Drawing.Color[,] sourceColours;

        public MainWindow()
        {
            InitializeComponent();

            community = new GenerationCycle();
            drawing = new Individual();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
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

            if (originalBitmap == null)
                return;


            this.bestGeneticImage.Source = null;

            using (var bit = new Bitmap(originalBitmap.Width, originalBitmap.Height))
            {
                Graphics g = Graphics.FromImage(bit);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                ImageRenderer.DrawImage(gui, g);

                this.bestGeneticImage.Source = Bitmap2BitmapImage(bit);
            }



            this.generation.Content = gui.Generation.ToString();

            this.fitness.Content = gui.Adaptation.ToString() + "%";

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (initialized == false)
            {
                community.InitializeEvolution(sourceColours);
                initialized = true;
            }

            this.GenerateButton.IsEnabled = false;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

                while (true)
                {
                    drawing = community.Generate();
                }

            }).Start();


        }

        public void EventIndividualFinished(object sender, IndividualEventArgs e)
        {
            Dispatcher.Invoke(delegate
            {
                using (var backBuffer = new Bitmap(originalBitmap.Width, originalBitmap.Height))
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
            sourceColours = new System.Drawing.Color[originalBitmap.Width, originalBitmap.Height];

            for (int i = 0; i < originalBitmap.Width; i++)
            {
                for (int j = 0; j < originalBitmap.Height; j++)
                {
                    var color = originalBitmap.GetPixel(i, j);
                    sourceColours[i, j] = color;
                }
            }
        }

        private void InsertOriginalImage(object sender, RoutedEventArgs e)
        {
            String originalImagePath = ImageOpener.ReadImageFromFile(FileFilter.IMAGE);

            if (originalImagePath != String.Empty)
            {
                originalBitmap = new Bitmap(originalImagePath);
                this.originalImage.Source = ImageOpener.Bitmap2BitmapImage(originalBitmap);
                OriginalImageColours();

                evolutionFitness = new EvolutionFitness(originalBitmap.Width, originalBitmap.Height);
                AlgorithmSettings.ImageWidth = originalBitmap.Width;
                AlgorithmSettings.ImageHeight = originalBitmap.Height;
            }
        }
    }
}
