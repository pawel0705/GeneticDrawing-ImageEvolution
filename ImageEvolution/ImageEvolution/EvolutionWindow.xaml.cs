﻿using ImageEvolution.Model.Genetic.Evolution;
using ImageEvolution.Model.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;

using static ImageEvolution.ImageOpener;
using System.Collections.Generic;
using ImageEvolution.ViewModel;
using System.Text;

namespace ImageEvolution
{
    public partial class EvolutionWindow : UserControl
    {
        private Bitmap _originalBitmap;
        internal TwoParentEvolution _community;
        internal SingleParentEvolution _oneIndividual;

        internal bool algorithmStarted = false;

        private bool fileDNAloaded = false;

        private bool _generateButtonLock = false;
        private bool _stopButtonLock = false;

        private bool _newBestIndividual = false;
        private bool _evolutionInitialized = false;

        internal int _seconds = 0;
        internal int _minutes = 0;

        internal int _generation = 0;
        internal int _killedChilds = 0;
        internal double _bestFitness = 0;
        internal double _currentFitness = 0;
        internal int _mutationChance = 0;
        internal int _dunamicMutation = 0;
        internal int _mutationType = 0;
        internal int _shapesAmount = 0;

        private bool dnaLoaded = false;

        private List<IndividualListData> individualListDatas;

        private Individual _tmpIndividual;
        private Individual _topGenerationIndividual;
        private Individual _bestOfAllIndividual;

        private Color[,] sourceColours;

        private DispatcherTimer _timeTimer;

        private Thread _generationThread;

        internal List<ChartKeyValue> dataChartList;

        public bool generateTwoParent;

        public EvolutionWindow()
        {
            InitializeComponent();

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
            _community.IndividualCreated += ShowDataImages;

            _oneIndividual = new SingleParentEvolution();
            _oneIndividual.IndividualCreated += ShowDataImages;

            _tmpIndividual = new Individual();

            dataChartList = new List<ChartKeyValue>();
        }

        private void InitializeTimers()
        {
            _timeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
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

        private void ShowDataImages(object sender, IndividualEventArgs e)
        {
            Dispatcher.Invoke(delegate
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

                        dataChartList.Add(new ChartKeyValue() { Key = _bestOfAllIndividual.Generation, Value = _bestOfAllIndividual.Adaptation });
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

                _generation = _topGenerationIndividual.Generation;
                _killedChilds = AlgorithmInformation.KilledChilds;
                this._bestFitness = Math.Round(_bestOfAllIndividual.Adaptation, 2);
                this._currentFitness = Math.Round(_topGenerationIndividual.Adaptation, 2);
                this._mutationChance = AlgorithmInformation.MutationChance;
                this._mutationType = (int)AlgorithmInformation.MutationType;


                this.actualGeneticImage.Source = bitmap;
                this.generation.Content = "Generation: " + _topGenerationIndividual.Generation.ToString();
                this.currentFitness.Content = "Current fitness: " + Math.Round(_topGenerationIndividual.Adaptation, 2).ToString() + "%";
                this.populationLabel.Content = "Population: " + AlgorithmInformation.Population;
                this.eliteLabel.Content = "Elite: " + AlgorithmInformation.Elite;
                this.mutationTypeLabel.Content = "Mutation type: " + AlgorithmInformation.MutationType.ToString();
                this.mutationChanceLabel.Content = "Mutation chance: " + AlgorithmInformation.MutationChance.ToString() + "%";
                this.killedChildsLabel.Content = "Killed childs: " + AlgorithmInformation.KilledChilds;

                _shapesAmount = (int)this.ShapesAmountSlider.Value;

                if (this.mutationDynamicRadio.IsChecked ?? false)
                {
                    this.dinamicallyMutationLabel.Content = "Dynamic mutation: YES";
                    this._dunamicMutation = 1;
                }
                else
                {
                    this.dinamicallyMutationLabel.Content = "Dynamic mutation: NO";
                    this._dunamicMutation = 0;
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
            });
        }

        private void EnableUI(bool enable)
        {
            InsertImageButton.IsEnabled = enable;
            LoadDNAButton.IsEnabled = enable;

            circleCheckBox.IsEnabled = enable;
            pentagonCheckBox.IsEnabled = enable;
            rectangleCheckBox.IsEnabled = enable;
            triangleCheckBox.IsEnabled = enable;

            mutationDynamicRadio.IsEnabled = enable;
            mutationStaticRadio.IsEnabled = enable;

            mutationSoftRadio.IsEnabled = enable;
            mutationHardRadio.IsEnabled = enable;
            mutationMediumRadio.IsEnabled = enable;
            mutationGaussianRadio.IsEnabled = enable;

            singleParentRadio.IsEnabled = enable;
            twoParentRadio.IsEnabled = enable;

            EliteAmountSlider.IsEnabled = enable;
            MutationAmountSlider.IsEnabled = enable;
            PopulationAmountSlider.IsEnabled = enable;
            ShapesAmountSlider.IsEnabled = enable;

            eliteAmountTextBox.IsEnabled = enable;
            mutationAmountTextBox.IsEnabled = enable;
            populationAmountTextBox.IsEnabled = enable;
            shapesAmountTextBox.IsEnabled = enable;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!dnaLoaded)
            {


                if (!(circleCheckBox.IsChecked ?? false) && !(pentagonCheckBox.IsChecked ?? false) &&
                    !(rectangleCheckBox.IsChecked ?? false) && !(triangleCheckBox.IsChecked ?? false))
                {
                    MessageBox.Show("You must select at least one type of shape!", "No shape selected", MessageBoxButton.OK, MessageBoxImage.Information);

                    return;
                }

                if (originalImage.Source == null)
                {
                    MessageBox.Show("You must first insert an image!", "No image selected", MessageBoxButton.OK, MessageBoxImage.Information);

                    return;
                }

                if (!(mutationDynamicRadio.IsChecked ?? false) && !(mutationStaticRadio.IsChecked ?? false))
                {
                    MessageBox.Show("Choose chance of mutation type!", "Missing informations", MessageBoxButton.OK, MessageBoxImage.Information);

                    return;
                }

                if (!(mutationSoftRadio.IsChecked ?? false) && !(mutationMediumRadio.IsChecked ?? false) &&
                    !(mutationHardRadio.IsChecked ?? false) && !(mutationGaussianRadio.IsChecked ?? false))
                {
                    MessageBox.Show("Choose the type of mutation!", "Missing informations", MessageBoxButton.OK, MessageBoxImage.Information);

                    return;
                }

                if (!(singleParentRadio.IsChecked ?? false) && !(twoParentRadio.IsChecked ?? false))
                {
                    MessageBox.Show("Choose the type of algorithm!", "Missing informations", MessageBoxButton.OK, MessageBoxImage.Information);

                    return;
                }

            }

            if (_generateButtonLock == false)
            {
                if (_evolutionInitialized == false)
                {
                    AlgorithmInformation.CircleShape = circleCheckBox.IsChecked ?? false;
                    AlgorithmInformation.PentagonShape = pentagonCheckBox.IsChecked ?? false;
                    AlgorithmInformation.SquareShape = rectangleCheckBox.IsChecked ?? false;
                    AlgorithmInformation.TriangleShape = triangleCheckBox.IsChecked ?? false;

                    AlgorithmInformation.DynamicMutation = mutationDynamicRadio.IsChecked ?? false;

                    AlgorithmInformation.ShapesAmount = (int)ShapesAmountSlider.Value;
                    AlgorithmInformation.Population = (int)PopulationAmountSlider.Value;
                    AlgorithmInformation.Elite = (int)(AlgorithmInformation.Population * (EliteAmountSlider.Value / 100.0f));
                    AlgorithmInformation.MutationChance = (int)MutationAmountSlider.Value;

                    if (mutationSoftRadio.IsChecked ?? false)
                    {
                        AlgorithmInformation.MutationType = MutationType.SOFT;
                    }
                    else if (mutationMediumRadio.IsChecked ?? false)
                    {
                        AlgorithmInformation.MutationType = MutationType.MEDIUM;
                    }
                    else if (mutationHardRadio.IsChecked ?? false)
                    {
                        AlgorithmInformation.MutationType = MutationType.HARD;
                    }
                    else
                    {
                        AlgorithmInformation.MutationType = MutationType.GAUSSIAN;
                    }

                    EnableUI(false);

                    if (fileDNAloaded == true)
                    {
                        _oneIndividual.InitializeFromDNA(sourceColours, individualListDatas, _generation);
                        /*
                        if(individualListDatas.Count == 1)
                        {
                            _oneIndividual.InitializeFromDNA(sourceColours, individualListDatas);
                        }
                        */
                    }
                    else
                    {
                        if (twoParentRadio.IsChecked ?? false)
                        {
                            generateTwoParent = true;
                            _community.InitializeEvolution(sourceColours);
                        }
                        else
                        {
                            generateTwoParent = false;
                            _oneIndividual.InitializeEvolution(sourceColours);
                        }
                    }

                    _evolutionInitialized = true;
                }

                this.algorithmStarted = true;

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
                    if (generateTwoParent)
                    {
                        GenerateImagesTwoParentReproduction();
                    }
                    else
                    {
                        GenerateImagesSingleParentReproduction();
                    }
                }
                catch (ThreadInterruptedException ex)
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

        private void OriginalImageColours()
        {
            sourceColours = new Color[_originalBitmap.Width, _originalBitmap.Height];

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

                AlgorithmInformation.ImageWidth = _originalBitmap.Width;
                AlgorithmInformation.ImageHeight = _originalBitmap.Height;

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
            AlgorithmInformation.ShapesAmount = (int)ShapesAmountSlider.Value;
        }

        private void PopulationAmountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AlgorithmInformation.Population = (int)PopulationAmountSlider.Value;

            AlgorithmInformation.Elite = (int)(AlgorithmInformation.Population * (EliteAmountSlider.Value / 100.0f));

            if (AlgorithmInformation.Elite == 0)
            {
                AlgorithmInformation.Elite = 1;
            }
        }

        private void EliteAmountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AlgorithmInformation.Elite = (int)(AlgorithmInformation.Population * (EliteAmountSlider.Value / 100.0f));

            if (AlgorithmInformation.Elite == 0)
            {
                AlgorithmInformation.Elite = 1;
            }
        }

        private void ResetApp()
        {
            _stopButtonLock = true;
            StopButton.IsEnabled = false;
            _generateButtonLock = false;

            _timeTimer.Stop();

            RemoveThread();
            EnableUI(true);

            this.algorithmStarted = false;

            this.actualGeneticImage.Source = null;
            this.bestGeneticImage.Source = null;
            this.originalImage.Source = null;

            this.generation.Content = "Generation: 0";
            this.currentFitness.Content = "Current fitness: 0%";
            this.populationLabel.Content = "Population: 0";
            this.eliteLabel.Content = "Elite: 0";
            this.mutationTypeLabel.Content = "Mutation type: -";
            this.mutationChanceLabel.Content = "Mutation chance: 0%";
            this.killedChildsLabel.Content = "Killed childs: 0";
            this.dinamicallyMutationLabel.Content = "Dynamic mutation: -";
            this.reproductionTypeLabel.Content = "Reproduction: -";
            this.bestFitness.Content = "Best fitness: 0%";

            this._seconds = 0;
            this._minutes = 0;
            this.elapsedTime.Content = "Elapsed time: 0:00";

            this._generation = 0;
            this._bestFitness = 0;
            this._currentFitness = 0;
            this._mutationChance = 0;
            this._dunamicMutation = 0;
            this._mutationType = 0;

            this._originalBitmap = null;

            circleCheckBox.IsChecked = false;
            pentagonCheckBox.IsChecked = false;
            rectangleCheckBox.IsChecked = false;
            triangleCheckBox.IsChecked = false;

            mutationDynamicRadio.IsChecked = false;
            mutationStaticRadio.IsChecked = false;

            mutationSoftRadio.IsChecked = false;
            mutationHardRadio.IsChecked = false;
            mutationMediumRadio.IsChecked = false;
            mutationGaussianRadio.IsChecked = false;

            singleParentRadio.IsChecked = false;
            twoParentRadio.IsChecked = false;

            _evolutionInitialized = false;
            _newBestIndividual = true;

            AlgorithmInformation.DynamicMutation = false;
            AlgorithmInformation.Elite = 0;
            AlgorithmInformation.ImageHeight = 0;
            AlgorithmInformation.ImageWidth = 0;
            AlgorithmInformation.KilledChilds = 0;
            AlgorithmInformation.MutationChance = 0;
            AlgorithmInformation.MutationType = MutationType.VOID;
            AlgorithmInformation.Population = 0;
            AlgorithmInformation.SquareShape = false;
            AlgorithmInformation.TriangleShape = false;
            AlgorithmInformation.PentagonShape = false;
            AlgorithmInformation.CircleShape = false;

            InitializeObjects();

            _tmpIndividual = null;
            _topGenerationIndividual = null;
            _bestOfAllIndividual = null;

            dnaLoaded = false;
            fileDNAloaded = false;
            this.DNAfileInformation.Content = "DNA not loaded";
        }

        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {
            ResetApp();
        }

        private void MutationAmountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AlgorithmInformation.MutationChance = (int)MutationAmountSlider.Value;
        }

        private void LoadDNAButtonClick(object sender, RoutedEventArgs e)
        {
            BinaryReader br;
            individualListDatas = new List<IndividualListData>();
            IndividualListData individualListData;
            try
            {
                br = new BinaryReader(new FileStream("DNAdata", FileMode.Open));


                int individualsNr = br.ReadInt32();

                for (int i = 0; i < individualsNr; i++)
                {
                    individualListData = new IndividualListData
                    {
                        Individual = br.ReadInt32(),
                        Fitness = br.ReadString(),
                        IndividualDNA = br.ReadString()
                    };

                    individualListDatas.Add(individualListData);
                }

                br.Close();

                BinaryReader br2;
                br2 = new BinaryReader(new FileStream("DNAdataInfo", FileMode.Open));

                _minutes = br2.ReadInt32();
                _seconds = br2.ReadInt32();
                _generation = br2.ReadInt32();
                _killedChilds = br2.ReadInt32();
                _bestFitness = br2.ReadDouble();
                _currentFitness = br2.ReadDouble();
                _mutationChance = br2.ReadInt32();
                _dunamicMutation = br2.ReadInt32();
                _mutationType = br2.ReadInt32();
                _shapesAmount = br2.ReadInt32();

                AlgorithmInformation.KilledChilds = _killedChilds;
                AlgorithmInformation.MutationChance = _mutationChance;
                AlgorithmInformation.MutationType = (MutationType)_mutationType;

                if (_dunamicMutation == 1)
                {
                    AlgorithmInformation.DynamicMutation = true;
                }
                else
                {
                    AlgorithmInformation.DynamicMutation = false;
                }

                this.generation.Content = "Generation: " + _generation;
                this.currentFitness.Content = "Current fitness: " + _currentFitness + "%";
                this.populationLabel.Content = "Population: 1";
                this.eliteLabel.Content = "Elite: 1";
                this.mutationTypeLabel.Content = "Mutation type: " + AlgorithmInformation.MutationType.ToString();
                this.mutationChanceLabel.Content = "Mutation chance: " + AlgorithmInformation.MutationChance.ToString() + "%";
                this.killedChildsLabel.Content = "Killed childs: " + AlgorithmInformation.KilledChilds;

                if (_dunamicMutation == 1)
                {

                    this.dinamicallyMutationLabel.Content = "Dynamic mutation: YES";
                }
                else
                {
                    this.dinamicallyMutationLabel.Content = "Dynamic mutation: NO";
                }

                this.reproductionTypeLabel.Content = "Reproduction: Single-parent";
                this.bestFitness.Content = "Best fitness: " + _bestFitness;

                PopulationAmountSlider.Value = 1;
                EliteAmountSlider.Value = 1;
                ShapesAmountSlider.Value = _shapesAmount;
                MutationAmountSlider.Value = _mutationChance;

                this.elapsedTime.Content = "Elapsed time: " + _minutes + ":" + _seconds;

                EnableUI(false);

                br2.Close();

                dnaLoaded = true;
                fileDNAloaded = true;

                InsertImageButton.IsEnabled = true;

                this.DNAfileInformation.Content = "DNA loaded!";
            }
            catch (IOException)
            {
                ResetApp();

                MessageBox.Show("There was a problem reading the DNA from file.", "Unable to read!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("DNA read from file. Now instert original image.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
