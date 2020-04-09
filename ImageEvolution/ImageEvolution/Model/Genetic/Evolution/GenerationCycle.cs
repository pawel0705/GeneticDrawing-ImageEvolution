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

        public void InitializeEvolution(Color[,] sourceIndividual)
        {
            _populationIndividuals = new Individual[AlgorithmSettings.Population];
            _eliteIndividuals = new Individual[AlgorithmSettings.Elite];

            _destinationIndividual = sourceIndividual;

            _evolutionFitness = new EvolutionFitness(Utils.AlgorithmSettings.ImageWidth, Utils.AlgorithmSettings.ImageHeight);

            for (int i = 0; i < AlgorithmSettings.Population; i++)
            {
                var individual = new Individual();
                individual.Initialize();
                _evolutionFitness.CompareImages(individual, _destinationIndividual);

                _populationIndividuals[i] = individual;
            }
        }


        public Individual Generate()
        {
            for (int j = 0; j < AlgorithmSettings.Population; j++)
            {
                _evolutionFitness.CompareImages(_populationIndividuals[j], _destinationIndividual);
            }

            Individual[] sortedGeneration = _populationIndividuals.OrderByDescending(i => i.Adaptation).ToArray();

            Array.Copy(sortedGeneration, 0, _eliteIndividuals, 0, AlgorithmSettings.Elite);

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

            EventIndividualFinished(_eliteIndividuals[0]);

            return _eliteIndividuals[0];
        }


        private Individual Reproduct(Individual mother, Individual father)
        {
            var individualChild = new Individual();

            for (int i = 0; i < mother.Shapes.Count; i++)
            {
                if (RandomMutation.RandomIntervalIntegerInclusive(0, 1) == 0)
                {
                    individualChild.Shapes.Add(mother.Shapes[i]);
                }
                else
                {
                    individualChild.Shapes.Add(father.Shapes[i]);
                }

                if(RandomMutation.RandomIntervalIntegerInclusive(0, AlgorithmSettings.MutationChance) == 1)
                {
                    individualChild.Shapes[i].MutateChromosome(MutationType.SOFT);
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
