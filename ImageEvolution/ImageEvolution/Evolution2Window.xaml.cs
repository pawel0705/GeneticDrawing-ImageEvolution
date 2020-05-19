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

        private DispatcherTimer updateListTime;

        public List<Individual> individualList;

        private List<IndividualListData> individualListDatas;

        public Evolution2Window()
        {
            InitializeComponent();

            individualList = new List<Individual>();

            individualListDatas = new List<IndividualListData>();

            updateListTime = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            updateListTime.Tick += UpdateIndividualList;


            updateListTime.Start();
        }

        private void UpdateIndividualList(object sender, EventArgs e)
        {

            lock (individualList)
            {
                individualListDatas.Clear();

                int iterator = 0;
                foreach (var i in individualList)
                {
                    individualListDatas.Add(new IndividualListData() { Individual = iterator, Fitness = i.Adaptation.ToString(), IndividualDNA = i.DNAstring.ToString() }); ;

                    iterator++;
                }

                lvIndividuals.ItemsSource = individualListDatas;
                
            }

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
