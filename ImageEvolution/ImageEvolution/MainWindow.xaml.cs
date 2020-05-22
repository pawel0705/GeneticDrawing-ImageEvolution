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
    public partial class MainWindow : Window
    {
        internal EvolutionWindow evolutionWindow;
        private Evolution2Window evolution2Window;

        public MainWindow()
        {
            evolutionWindow = new EvolutionWindow();
            evolution2Window = new Evolution2Window(this);

            InitializeComponent();
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (50 + (60 * index)), 0, 0);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SetDataBetweenWindows()
        {
            evolution2Window.individualList.Clear();

            if (evolutionWindow.generateTwoParent)
            {
                for(int i = 0; i < AlgorithmInformation.Population; i++)
                {
                    evolution2Window.individualList.Add(evolutionWindow._community._populationIndividuals[i]);
                }
            }
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    GridWindow.Children.Clear();
                    GridWindow.Children.Add(evolutionWindow);
                    break;
                case 1:
                    GridWindow.Children.Clear();
                    GridWindow.Children.Add(evolution2Window);
                    SetDataBetweenWindows();
                    break;
                default:
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}
