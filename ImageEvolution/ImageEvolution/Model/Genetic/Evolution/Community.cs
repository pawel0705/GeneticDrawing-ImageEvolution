using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Settings;

namespace ImageEvolution.Model.Genetic.Evolution
{
    class Community
    {
        private int _population = 50;
        private int _elite = 10;

        private float mutationSize = 10.0f; // 0 - 100

        ImageDNA[] geneticImages;
        ImageDNA[] eliteImages;
        double[] evolutionSummary;

        Color[,] originalImages;

        EvolutionFitness evolutionFitness;

        enum Mutation
        {
            VOID = 0, // brak mutacji
            SOFT = 1, // zmien dokladnie 1 parametr (R G B A X Y) o mala wartosc
            MEDIUM = 2, // zmien dokladnie 1 parametr o LOSOWĄ wartosc
            HARD = 3, // zmien WSZYSTKIE parametry o 1 LOSOWĄ wartosc
            GAUSSIAN = 4, // zmien jeden parametr o wartosc wzieta z "normal distribution"
            BIT_STRING = 5, // 
            FLIP_BIT = 6,
            BOUNDARY = 7,
            NON_UNIFORM = 8,
            UNIFORM = 9,
            SHRINK = 10
        }

        public void InitializeEvolution(Color[,] sourceColours)
        {
            geneticImages = new ImageDNA[_population];
            eliteImages = new ImageDNA[_elite];

            originalImages = sourceColours;

            evolutionFitness = new EvolutionFitness(Utils.AlgorithmSettings.ImageWidth, Utils.AlgorithmSettings.ImageHeight);

            evolutionSummary = new double[_population];

            for (int i = 0; i < _population; i++)
            {
                /*
                ImageDNA drawing = new ImageDNA();
                drawing.Initialize();

                var backBuffer = new Bitmap(originalBitmap.Width, originalBitmap.Height);

                Graphics g = Graphics.FromImage(backBuffer);

                ImageRenderer.DrawImage(drawing, g);
                this.bestGeneticImage.Source = Bitmap2BitmapImage(backBuffer);

                this.fitness.Content = evolutionFitness.CompareImages(drawing, sourceColours).ToString();
                */
                var drawing = new ImageDNA();
                drawing.Initialize();

                geneticImages[i] = drawing;
            }

        }

        private void Generate()
        {
            for(int i = 0; i < _population; i++)
            {
                evolutionSummary[i] = evolutionFitness.CompareImages(geneticImages[i], originalImages);
            }

            // znajdz elite
            Array.Sort(evolutionSummary);
            Array.Reverse(evolutionSummary);
            Array.Copy(evolutionSummary, 0, eliteImages, 0, 10);
        }

        private double GaussNormalDistribution()
        {
            var x = RandomMutation.RandomNumber();
            var y = RandomMutation.RandomNumber();

            var r = 0.003 * mutationSize * Math.Sqrt(-2.0f * Math.Log(x)) * Math.Cos(2.0 * Math.PI * y);

            if (r < -1.0 )
            {
                r = 0;
            }

            if(r > 1.0)
            {
                r = 0;
            }

            return r;
        }
    }
}
