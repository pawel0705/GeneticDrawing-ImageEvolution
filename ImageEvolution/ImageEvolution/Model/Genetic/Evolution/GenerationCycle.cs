using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Settings;
using ImageEvolution.Model.Utils;

namespace ImageEvolution.Model.Genetic.Evolution
{
    public class IndividualEventArgs : EventArgs
    {
        public Individual individual { get; set; }
    }
    

     public class GenerationCycle
    {
        private Individual[] _populationIndividuals;
        private Individual[] _eliteIndividuals;

        private Color[,] _destinationIndividual;

        private EvolutionFitness _evolutionFitness;

        private int _generation = 0;

        public void InitializeEvolution(Color[,] sourceIndividual)
        {
            _populationIndividuals = new Individual[AlgorithmSettings.Population];
            _eliteIndividuals = new Individual[AlgorithmSettings.Elite];

            _destinationIndividual = sourceIndividual;

            _evolutionFitness = new EvolutionFitness(Utils.AlgorithmSettings.ImageWidth, Utils.AlgorithmSettings.ImageHeight);

            Individual individual;

            for (int i = 0; i < AlgorithmSettings.Population; i++)
            {
                individual = new Individual();
                individual.Initialize();

                _populationIndividuals[i] = individual;
            }

        }


        public Individual Generate()
        {
            _generation++;

            for (int j = 0; j < AlgorithmSettings.Population; j++)
            {
                _populationIndividuals[j].Generation = _generation;
                _evolutionFitness.CompareImages(_populationIndividuals[j], _destinationIndividual);
            }

            Individual[] sortedGeneration = new Individual[AlgorithmSettings.Population];
            for(int j = 0; j < AlgorithmSettings.Population; j++)
            {
                sortedGeneration[j] = _populationIndividuals[j].CloneIndividual();
            }

            sortedGeneration = sortedGeneration.OrderByDescending(i => i.Adaptation).ToArray();

            for(int j = 0; j < AlgorithmSettings.Elite; j++)
            {
                _eliteIndividuals[j] = sortedGeneration[j];
            }

            int i = 0;
            int mother = 0;
            int father = 1;

            while (i < AlgorithmSettings.Population)
            {
                _populationIndividuals[i] = Reproduct(_eliteIndividuals[mother], _eliteIndividuals[father]);

                mother++;
                if(mother >= AlgorithmSettings.Elite)
                {
                    father++;
                    mother = 0;
                }

                if(father >= AlgorithmSettings.Elite)
                {
                    father = 0;
                }

                i++;
                if(i > AlgorithmSettings.Population)
                {
                    break;
                }
            }

            if(_generation > 350)
            {
                AlgorithmSettings.MutationChance = 50;
            }
            if(_generation > 700)
            {
                AlgorithmSettings.MutationChance = 100;
            }
            if(_generation > 1000)
            {
                AlgorithmSettings.MutationChance = 200;
            }

            EventIndividualFinished(_eliteIndividuals[0]);

            return _eliteIndividuals[0];
        }


        private Individual Reproduct(Individual mother, Individual father)
        {
            var individualChild = new Individual();


            // triangle shapes
            for (int i = 0; i < mother.TriangleShapes.Count; i++)
            {
                if (RandomMutation.RandomIntervalIntegerInclusive(0, 1) == 0)
                {
                    individualChild.TriangleShapes.Add(mother.TriangleShapes[i]);
                }
                else
                {
                    individualChild.TriangleShapes.Add(father.TriangleShapes[i]);
                }

                if(RandomMutation.RandomIntervalIntegerInclusive(0, AlgorithmSettings.MutationChance) == 1)
                {
                    individualChild.TriangleShapes[i].MutateChromosome(MutationType.SOFT);
                }
            }

            // square shapes
            for (int i = 0; i < mother.SquareShapes.Count; i++)
            {
                if (RandomMutation.RandomIntervalIntegerInclusive(0, 1) == 0)
                {
                    individualChild.SquareShapes.Add(mother.SquareShapes[i]);
                }
                else
                {
                    individualChild.SquareShapes.Add(father.SquareShapes[i]);
                }

                if (RandomMutation.RandomIntervalIntegerInclusive(0, AlgorithmSettings.MutationChance) == 1)
                {
                    individualChild.SquareShapes[i].MutateChromosome(MutationType.SOFT);
                }
            }

            // elipse shapes
            for (int i = 0; i < mother.ElipseShapes.Count; i++)
            {
                if (RandomMutation.RandomIntervalIntegerInclusive(0, 1) == 0)
                {
                    individualChild.ElipseShapes.Add(mother.ElipseShapes[i]);
                }
                else
                {
                    individualChild.ElipseShapes.Add(father.ElipseShapes[i]);
                }

                if (RandomMutation.RandomIntervalIntegerInclusive(0, AlgorithmSettings.MutationChance) == 1)
                {
                    individualChild.ElipseShapes[i].MutateChromosome(MutationType.SOFT);
                }
            }



            return individualChild;
        }

        public event EventHandler<IndividualEventArgs> IndividualCreated;

        protected virtual void EventIndividualFinished(Individual individual)
        {
            IndividualCreated?.Invoke(this, new IndividualEventArgs() { individual = individual });
        }
    }
}
