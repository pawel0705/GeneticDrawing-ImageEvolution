using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Genetic.Evolution;
using ImageEvolution.Model.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace ImageEvolution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap originalBitmap;
        EvolutionFitness evolutionFitness;
        GenerationCycle community;
        bool initialized = false;
        Individual drawing;

        private System.Drawing.Color[,] sourceColours;

        public MainWindow()
        {
            InitializeComponent();

            community = new GenerationCycle();
            drawing = new Individual();

            this.community.IndividualCreated += this.EventIndividualFinished;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (initialized == false)
            {
                community.InitializeEvolution(sourceColours);
                initialized = true;
            }

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                while(true)
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
                        this.bestGeneticImage.Source = Bitmap2BitmapImage(backBuffer);

                        this.fitness.Content = drawing.Adaptation.ToString() + "%";
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
