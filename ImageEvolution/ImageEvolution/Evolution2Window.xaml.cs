using ImageEvolution.Model.Genetic.Evolution;
using ImageEvolution.ViewModel;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ImageEvolution
{


    public partial class Evolution2Window : UserControl
    {
        private MainWindow _mainWindow;

        public List<Individual> individualList;

        private List<IndividualListData> individualListDatas;

        public Evolution2Window(MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            individualList = new List<Individual>();

            individualListDatas = new List<IndividualListData>();

        }

        private void UpdateIndividualList(object sender, EventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InsertOriginalImage(object sender, RoutedEventArgs e)
        {

        }

        private void PrintInformationIndividual(object sender, RoutedEventArgs e)
        {
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
                individualListDatas.Add(new IndividualListData() { Individual = iterator, Fitness = i.Adaptation.ToString(), IndividualDNA = i.DNAstring.ToString() }); ;

                iterator++;
            }

            lvIndividuals.ItemsSource = individualListDatas;
        }

        private void SaveDNAButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveDNAToFile(object sender, RoutedEventArgs e)
        {
            BinaryWriter bw;

            try
            {
                bw = new BinaryWriter(new FileStream("DNAdata", FileMode.Create));

                bw.Write(individualListDatas.Count);

                foreach (var dna in individualListDatas)
                {
                    bw.Write(dna.Individual);
                    bw.Write(dna.Fitness);
                    bw.Write(dna.IndividualDNA);
                }

                bw.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show("There was a problem writing the DNA to file.", "Unable to save!", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            MessageBox.Show("DNA saved to file.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshChartButtonClick(object sender, RoutedEventArgs e)
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



            dynamicChart.LegendStyle = styleLegand;

            GroupBoxDynamicChart.Content = dynamicChart;
        }
    }
}
