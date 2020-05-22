using ImageEvolution.Model.Settings;
using ImageEvolution.Model.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEvolution.Model.Genetic.Evolution
{
    public abstract class Evolution
    {
        protected int _generation = 0;

        protected Color[,] _destinationIndividual;

        protected EvolutionFitness _evolutionFitness;

        public event EventHandler<IndividualEventArgs> IndividualCreated;

        protected virtual void OnIndividualCreated(Individual individual)
        {
            IndividualCreated?.Invoke(this, new IndividualEventArgs() { Individual = individual });
        }

        public abstract Individual Generate();

        protected bool WillMutate()
        {
            if (AlgorithmInformation.MutationChance <= 0)
            {
                AlgorithmInformation.MutationChance = 1;
            }

            if (RandomMutation.RandomIntervalIntegerInclusive(0, 100 - AlgorithmInformation.MutationChance) == 0)
            {
                return true;
            }

            return false;
        }
    }
}
