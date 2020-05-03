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
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Reflection;

namespace ImageEvolution
{
    public partial class EvolutionWindow : UserControl
    {
        private Bitmap _originalBitmap;
        private TwoParentEvolution _community;
        private SingleParentEvolution _oneIndividual;

        private bool _generateButtonLock = false;
        private bool _insertImageButtonLock = false;
        private bool _stopButtonLock = false;

        private bool _newBestIndividual = false;
        private bool _evolutionInitialized = false;

        private int _milisecondsTime = 10;
        private int _seconds = 0;
        private int _minutes = 0;

        private Individual _tmpIndividual;
        private Individual _topGenerationIndividual;
        private Individual _bestOfAllIndividual;

        private System.Drawing.Color[,] sourceColours;

        private DispatcherTimer _refreshImagesTimer;
        private DispatcherTimer _timeTimer;

        private Thread _generationThread;

        private bool generateTwoParent;

        public EvolutionWindow()
        {
            InitializeComponent();

            // this.community.IndividualCreated += this.EventIndividualFinished;
            InitializeObjects();
            InitializeTimers();
            InitializeButtons();
            InitializeSliders();
        }

        private void InitializeSliders()
        {
            PopulationAmountSlider.Value = 50;
            EliteAmountSlider.Value = 10;
            ShapesAmountSlider.Value = 100;
            MutationAmountSlider.Value = 5;
        }

        private void InitializeObjects()
        {
            _community = new TwoParentEvolution();
            _oneIndividual = new SingleParentEvolution();

            _tmpIndividual = new Individual();
        }

        private void InitializeTimers()
        {
            _refreshImagesTimer = new DispatcherTimer();
            _refreshImagesTimer.Interval = TimeSpan.FromMilliseconds(_milisecondsTime);
            _refreshImagesTimer.Tick += ShowDataImages;
            _refreshImagesTimer.Start();

            _timeTimer = new DispatcherTimer();
            _timeTimer.Interval = TimeSpan.FromSeconds(1);
            _timeTimer.Tick += UpdateTime;
        }

        private void InitializeButtons()
        {
            GenerateButton.IsEnabled = false;
            _generateButtonLock = false;

            StopButton.IsEnabled = false;
            _stopButtonLock = false;
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            _seconds++;

            if (_seconds < 10)
            {
                this.elapsedTime.Content = "Elapsed time: " + _minutes + ":" + "0" + _seconds;
            }
            else if (_seconds >= 10 && _seconds < 60)
            {
                this.elapsedTime.Content = "Elapsed time: " + _minutes + ":" + _seconds;
            }
            else if (_seconds >= 60)
            {
                _seconds = 0;
                _minutes++;
                this.elapsedTime.Content = "Elapsed time: " + _minutes + ":" + "00";
            }


        }

        private void ShowDataImages(object sender, EventArgs e)
        {
            if (_tmpIndividual == null || _originalBitmap == null)
            {
                return;
            }

            lock (_tmpIndividual)
            {
                if (_bestOfAllIndividual == null)
                {
                    _bestOfAllIndividual = _tmpIndividual.CloneIndividual();
                    _newBestIndividual = true;
                }

                if (_bestOfAllIndividual.Adaptation < _tmpIndividual.Adaptation)
                {
                    _bestOfAllIndividual = _tmpIndividual.CloneIndividual();
                    _newBestIndividual = true;
                }

                _topGenerationIndividual = _tmpIndividual.CloneIndividual();
            }

            using (var bit = new Bitmap(_originalBitmap.Width, _originalBitmap.Height))
            {
                this.actualGeneticImage.Source = null;
                Graphics graphics = Graphics.FromImage(bit);
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                ImageRenderer.DrawImage(_topGenerationIndividual, graphics);
                bit.Save("topGenerationIndividual.png", ImageFormat.Png);

                if (_newBestIndividual == true)
                {
                    this.bestGeneticImage.Source = null;
                    bit.Save("topIndividual.png", ImageFormat.Png);
                }
            }

            var bitmap = new BitmapImage();
            var stream = File.OpenRead("topGenerationIndividual.png");

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            stream.Close();
            stream.Dispose();

            this.actualGeneticImage.Source = bitmap;
            this.generation.Content = "Generation: " + _topGenerationIndividual.Generation.ToString();
            this.currentFitness.Content = "Current fitness: " + Math.Round(_topGenerationIndividual.Adaptation, 2).ToString() + "%";
            this.populationLabel.Content = "Population: " + AlgorithmSettings.Population;
            this.eliteLabel.Content = "Elite: " + AlgorithmSettings.Elite;
            this.mutationTypeLabel.Content = "Mutation type: " + AlgorithmSettings.MutationType.ToString();
            this.mutationChanceLabel.Content = "Mutation chance: " + AlgorithmSettings.MutationChance.ToString() + "%";

            if(this.mutationDynamicRadio.IsChecked ?? false)
            {
                this.dinamicallyMutationLabel.Content = "Dynamic mutation: YES";
            }
            else
            {
                this.dinamicallyMutationLabel.Content = "Dynamic mutation: NO";
            }

            if (this.twoParentRadio.IsChecked ?? false)
            {
                this.reproductionTypeLabel.Content = "Reproduction: Two-parents";
            }
            else
            {
                this.reproductionTypeLabel.Content = "Reproduction: Single-parent";
            }


            if (_newBestIndividual == true)
            {
                var bitmap1 = new BitmapImage();
                var stream1 = File.OpenRead("topIndividual.png");

                bitmap1.BeginInit();
                bitmap1.CacheOption = BitmapCacheOption.OnLoad;
                bitmap1.StreamSource = stream1;
                bitmap1.EndInit();
                stream1.Close();
                stream1.Dispose();

                this.bestGeneticImage.Source = bitmap1;
                this.bestFitness.Content = "Best fitness: " + Math.Round(_bestOfAllIndividual.Adaptation, 2).ToString() + "%";

                _newBestIndividual = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_generateButtonLock == false)
            {
                if(_evolutionInitialized == false)
                {
                    AlgorithmSettings.CircleShape = circleCheckBox.IsChecked ?? false;
                    AlgorithmSettings.PentagonShape = pentagonCheckBox.IsChecked ?? false;
                    AlgorithmSettings.SquareShape = rectangleCheckBox.IsChecked ?? false;
                    AlgorithmSettings.TriangleShape = triangleCheckBox.IsChecked ?? false;

                    AlgorithmSettings.DynamicMutation = mutationDynamicRadio.IsChecked ?? false;

                    AlgorithmSettings.ShapesAmount = (int)ShapesAmountSlider.Value;
                    AlgorithmSettings.Population = (int)PopulationAmountSlider.Value;
                    AlgorithmSettings.Elite = (int)(AlgorithmSettings.Population * (EliteAmountSlider.Value / 100.0f));
                    AlgorithmSettings.MutationChance = (int)MutationAmountSlider.Value;

                    if (mutationSoftRadio.IsChecked ?? false)
                    {
                        AlgorithmSettings.MutationType = MutationType.SOFT;
                    }
                    else if(mutationMediumRadio.IsChecked ?? false)
                    {
                        AlgorithmSettings.MutationType = MutationType.MEDIUM;
                    }
                    else if(mutationHardRadio.IsChecked ?? false)
                    {
                        AlgorithmSettings.MutationType = MutationType.HARD;
                    }
                    else
                    {
                        AlgorithmSettings.MutationType = MutationType.GAUSSIAN;
                    }

                    InsertImageButton.IsEnabled = false;

                    circleCheckBox.IsEnabled = false;
                    pentagonCheckBox.IsEnabled = false;
                    rectangleCheckBox.IsEnabled = false;
                    triangleCheckBox.IsEnabled = false;

                    mutationDynamicRadio.IsEnabled = false;
                    mutationStaticRadio.IsEnabled = false;

                    mutationSoftRadio.IsEnabled = false;
                    mutationHardRadio.IsEnabled = false;
                    mutationMediumRadio.IsEnabled = false;
                    mutationGaussianRadio.IsEnabled = false;

                    singleParentRadio.IsEnabled = false;
                    twoParentRadio.IsEnabled = false;

                    EliteAmountSlider.IsEnabled = false;
                    MutationAmountSlider.IsEnabled = false;
                    PopulationAmountSlider.IsEnabled = false;
                    ShapesAmountSlider.IsEnabled = false;

                    eliteAmountTextBox.IsEnabled = false;
                    mutationAmountTextBox.IsEnabled = false;
                    populationAmountTextBox.IsEnabled = false;
                    shapesAmountTextBox.IsEnabled = false;

                    if(twoParentRadio.IsChecked ?? false)
                    {
                        generateTwoParent = true;
                        _community.InitializeEvolution(sourceColours);
                    }
                    else
                    {
                        generateTwoParent = false;
                        _oneIndividual.InitializeEvolution(sourceColours);
                    }


                    _evolutionInitialized = true;
                }

                this.GenerateButton.IsEnabled = false;
                _generateButtonLock = true;

                this.StopButton.IsEnabled = true;
                _stopButtonLock = false;

                _timeTimer.Start();
            }

            RemoveThread();

            _generationThread = new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.CurrentThread.Priority = ThreadPriority.Highest;

                try
                {
                    if(generateTwoParent)
                    {
                        GenerateImagesTwoParentReproduction();
                    }
                    else
                    {
                        GenerateImagesSingleParentReproduction();
                    }
                }
                catch(ThreadInterruptedException ex)
                {
                    // TODO ?
                }

            });
            _generationThread.Start();
        }

        private void RemoveThread()
        {
            if (_generationThread != null)
            {
                _generationThread.Interrupt();

                _generationThread.Join();
                _generationThread = null;
            }
        }

        private void GenerateImagesTwoParentReproduction()
        {
            while (!_stopButtonLock)
            {
                _tmpIndividual = _community.Generate();
            }
        }

        private void GenerateImagesSingleParentReproduction()
        {
            while (!_stopButtonLock)
            {
                _tmpIndividual = _oneIndividual.Generate();
            }
        }

        public void EventIndividualFinished(object sender, IndividualEventArgs e)
        {
            Dispatcher.Invoke(delegate
            {
                using (var backBuffer = new Bitmap(_originalBitmap.Width, _originalBitmap.Height))
                {
                    using (Graphics g = Graphics.FromImage(backBuffer))
                    {
                        ImageRenderer.DrawImage(_tmpIndividual, g);
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

                this.GenerateButton.IsEnabled = true;
                this._generateButtonLock = false;
            }
        }

        private void StopButtonClicki(object sender, RoutedEventArgs e)
        {
            _stopButtonLock = true;

            StopButton.IsEnabled = false;

            _generateButtonLock = false;
            GenerateButton.IsEnabled = true;

            _timeTimer.Stop();

            RemoveThread();
        }

  

        private void ShapesAmountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AlgorithmSettings.ShapesAmount = (int)ShapesAmountSlider.Value;
        }

        private void PopulationAmountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AlgorithmSettings.Population = (int)PopulationAmountSlider.Value;

            AlgorithmSettings.Elite = (int)(AlgorithmSettings.Population * (EliteAmountSlider.Value / 100.0f));

            if (AlgorithmSettings.Elite == 0)
            {
                AlgorithmSettings.Elite = 1;
            }
        }

        private void EliteAmountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AlgorithmSettings.Elite = (int)(AlgorithmSettings.Population * (EliteAmountSlider.Value / 100.0f));

            if(AlgorithmSettings.Elite == 0)
            {
                AlgorithmSettings.Elite = 1;
            }
        }

        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void MutationAmountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AlgorithmSettings.MutationChance = (int)MutationAmountSlider.Value;
        }
    }
}
