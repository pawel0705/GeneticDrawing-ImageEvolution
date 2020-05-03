using ImageEvolution.Model.Settings;
using ImageEvolution.Model.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageEvolution.Model.Genetic.Evolution
{
    class SingleParentEvolution
    {
        private Color[,] _destinationIndividual;

        private EvolutionFitness _evolutionFitness;

        private int _generation = 0;

        Individual _childIndividual;

        Individual _parentIndividual;

        public void InitializeEvolution(Color[,] sourceIndividual)
        {
            _childIndividual = new Individual();

            _parentIndividual = new Individual();

            _destinationIndividual = sourceIndividual;

            _evolutionFitness = new EvolutionFitness(AlgorithmSettings.ImageWidth, AlgorithmSettings.ImageHeight);

            _parentIndividual.Initialize();
        }

        public Individual Generate()
        {
            _generation++;

            if (AlgorithmSettings.DynamicMutation && (_generation % 10000 == 0))
            {
                AlgorithmSettings.MutationChance -= 1;
            }

            AlgorithmSettings.Population = 1;
            AlgorithmSettings.Elite = 1;

            _childIndividual = new Individual();

            for(int i = 0; i < _parentIndividual.TriangleShapes.Count; i++)
            {
                _childIndividual.TriangleShapes.Add(_parentIndividual.TriangleShapes[i]);

                if (WillMutate())
                {
                    _childIndividual.TriangleShapes[i].MutateChromosome();
                }
            }

            for (int i = 0; i < _parentIndividual.SquareShapes.Count; i++)
            {
                _childIndividual.SquareShapes.Add(_parentIndividual.SquareShapes[i]);

                if (WillMutate())
                {
                    _childIndividual.SquareShapes[i].MutateChromosome();
                }
            }

            for (int i = 0; i < _parentIndividual.ElipseShapes.Count; i++)
            {
                _childIndividual.ElipseShapes.Add(_parentIndividual.ElipseShapes[i]);

                if (WillMutate())
                {
                    _childIndividual.ElipseShapes[i].MutateChromosome();
                }
            }

            for (int i = 0; i < _parentIndividual.PentagonShapes.Count; i++)
            {
                _childIndividual.PentagonShapes.Add(_parentIndividual.PentagonShapes[i]);

                if (WillMutate())
                {
                    _childIndividual.PentagonShapes[i].MutateChromosome();
                }
            }

            _evolutionFitness.CompareImages(_childIndividual, _destinationIndividual);
            _childIndividual.Generation = _generation;

            if (_parentIndividual.Adaptation < _childIndividual.Adaptation)
            {
                _parentIndividual = _childIndividual.CloneIndividual();
            }

            return _childIndividual;
        }

        private bool WillMutate()
        {
            if (AlgorithmSettings.MutationChance <= 0)
            {
                AlgorithmSettings.MutationChance = 1;
            }

            if (RandomMutation.RandomIntervalIntegerInclusive(0, 100 - AlgorithmSettings.MutationChance) == 0)
            {
                return true;
            }

            return false;
        }
    }
}
