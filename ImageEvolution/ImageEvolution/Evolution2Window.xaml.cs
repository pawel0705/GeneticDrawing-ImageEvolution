using ImageEvolution.Model.Genetic.Evolution;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
        private class IndividualListData
        {
            public int Individual { get; set; }

            public string Fitness { get; set; }

            public string IndividualDNA { get; set; }
        }

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

            individualList.AddRange(_mainWindow.evolutionWindow._community.GetListOfIndividuals());
            

            int iterator = 0;
            foreach (var i in individualList)
            {
                individualListDatas.Add(new IndividualListData() { Individual = iterator, Fitness = i.Adaptation.ToString(), IndividualDNA = i.DNAstring.ToString() }); ;

                iterator++;
            }

            lvIndividuals.ItemsSource = individualListDatas;
        }
    }
}
