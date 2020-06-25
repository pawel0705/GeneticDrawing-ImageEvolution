using CsvHelper;
using ImageEvolution.Model.Genetic.Evolution;
using ImageEvolution.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace ImageEvolution
{
    public partial class Evolution2Window : UserControl
    {
        private MainWindow _mainWindow;

        public List<Individual> individualList;

        private readonly List<CsvData> csvData;

        private Thread _referehChartThread;

        private bool canRefreshChart;

        private bool canRefreshTable;

        private readonly List<IndividualListData> individualListDatas;

        public Evolution2Window(MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            individualList = new List<Individual>();

            individualListDatas = new List<IndividualListData>();

            csvData = new List<CsvData>();

            canRefreshChart = false;

            canRefreshTable = false;
        }

        private void PrintInformationIndividual(object sender, RoutedEventArgs e)
        {
            if (!_mainWindow.evolutionWindow.algorithmStarted)
            {
                canRefreshTable = false;

                MessageBox.Show("Run the algorithm first!", "Run algorithm first", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            canRefreshTable = true;

            individualListDatas.Clear();
            individualList.Clear();
            lvIndividuals.ItemsSource = null;

            if (_mainWindow.evolutionWindow.generateTwoParent)
            {
                individualList.AddRange(_mainWindow.evolutionWindow._community.GetListOfIndividuals());
            }
            else
            {
                individualList.Add(_mainWindow.evolutionWindow._oneIndividual._parentIndividual);
            }

            int iterator = 0;
            foreach (var i in individualList)
            {
                individualListDatas.Add(new IndividualListData() { Individual = iterator, Fitness = i.Adaptation.ToString(), IndividualDNA = i.DNAstring.ToString() });



                iterator++;
            }

            lvIndividuals.ItemsSource = individualListDatas;

        }


        private void SaveDNAToFile(object sender, RoutedEventArgs e)
        {
            if(!canRefreshTable)
            {
                MessageBox.Show(" First refresh the table to be able to save the DNA to the file!", "Unable to save!", MessageBoxButton.OK, MessageBoxImage.Error);
               
                return;
            }

            if(_mainWindow.evolutionWindow.generateTwoParent)
            {
                MessageBox.Show("DNA can only be saved for the 'Single - parent' algorithm!", "Unable to save!", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

    
            try
            {
                BinaryWriter bw = new BinaryWriter(new FileStream("DNAdata", FileMode.Create));

                bw.Write(individualListDatas.Count);

                foreach (var dna in individualListDatas)
                {
                    bw.Write(dna.Individual);
                    bw.Write(dna.Fitness);
                    bw.Write(dna.IndividualDNA);
                }

                bw.Close();

                BinaryWriter bw2 = new BinaryWriter(new FileStream("DNAdataInfo", FileMode.Create));

                bw2.Write(_mainWindow.evolutionWindow._minutes);
                bw2.Write(_mainWindow.evolutionWindow._seconds);
                bw2.Write(_mainWindow.evolutionWindow._generation);
                bw2.Write(_mainWindow.evolutionWindow._killedChilds);
                bw2.Write(_mainWindow.evolutionWindow._bestFitness);
                bw2.Write(_mainWindow.evolutionWindow._currentFitness);
                bw2.Write(_mainWindow.evolutionWindow._mutationChance);
                bw2.Write(_mainWindow.evolutionWindow._dunamicMutation);
                bw2.Write(_mainWindow.evolutionWindow._mutationType);
                bw2.Write(_mainWindow.evolutionWindow._shapesAmount);

                bw2.Close();

            }
            catch (IOException)
            {
                MessageBox.Show("There was a problem writing the DNA to file.", "Unable to save!", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("DNA saved to file.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshChartButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_mainWindow.evolutionWindow.algorithmStarted)
            {
                canRefreshChart = false;

                MessageBox.Show("Run the algorithm first!", "Run algorithm first", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            canRefreshChart = true;

            _referehChartThread = new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
            {
                Chart dynamicChart = new Chart();
                LineSeries lineseries = new LineSeries
                {
                    ItemsSource = _mainWindow.evolutionWindow.dataChartList,
                    DependentValuePath = "Value",
                    IndependentValuePath = "Key",

                };
                dynamicChart.Series.Add(lineseries);

                Style styleLegand = new Style { TargetType = typeof(Control) };
                styleLegand.Setters.Add(new Setter(Control.WidthProperty, 0d));
                styleLegand.Setters.Add(new Setter(Control.HeightProperty, 0d));


                csvData.Clear();
                foreach (var i in _mainWindow.evolutionWindow.dataChartList)
                {
                    csvData.Add(new CsvData { Adaptation = i.Value, Generation = i.Key });
                }

                dynamicChart.LegendStyle = styleLegand;

                GroupBoxDynamicChart.Content = dynamicChart;

            });
            });
            _referehChartThread.Start();
        }

        private void SaveGraphDataToFile(object sender, RoutedEventArgs e)
        {
            if(!canRefreshChart)
            {
                MessageBox.Show("First refresh the chart to be able to save the data from the chart to the CSV file!", "Can't save to CSV file", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            try
            {
                using var writer = new StreamWriter("graphData.csv");
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.Configuration.Delimiter = ";";
                csv.WriteRecords(csvData);

                MessageBox.Show("Data from the current chart has been saved to a CSV file.", "Save to CSV Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (IOException)
            {
                MessageBox.Show("There was a problem writing the graph data to cmv file.", "Unable to save!", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }
        }

        private void UpdateIndividualList(object sender, EventArgs e) { }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e) { }

        private void InsertOriginalImage(object sender, RoutedEventArgs e) { }

        private void SaveDNAButton_Click(object sender, RoutedEventArgs e) { }
    }
}
