using ImageEvolution.Model.Settings;
using ImageEvolution.Model.Utils;
using System.Drawing;

namespace ImageEvolution.Model.Genetic.Evolution
{
    class SingleParentEvolution
    {
        private Color[,] _destinationIndividual;

        private EvolutionFitness _evolutionFitness;

        private int _generation = 0;

        Individual _parentIndividual;

        public void InitializeEvolution(Color[,] sourceIndividual)
        {
            _parentIndividual = new Individual();
            _parentIndividual.Initialize();

            _destinationIndividual = sourceIndividual;

            _evolutionFitness = new EvolutionFitness(AlgorithmInformation.ImageWidth, AlgorithmInformation.ImageHeight);
        }

        public Individual Generate()
        {
            _generation++;

            if (AlgorithmInformation.DynamicMutation && (_generation % 10000 == 0))
            {
                AlgorithmInformation.MutationChance -= 1;
            }

            AlgorithmInformation.Population = 1;
            AlgorithmInformation.Elite = 1;

            var _childIndividual = new Individual();

            for(int i = 0; i < _parentIndividual.TriangleShapes.Count; i++)
            {
                _childIndividual.TriangleShapes.Add(_parentIndividual.TriangleShapes[i].CloneShapeChromosome());

                if (WillMutate())
                {
                    _childIndividual.TriangleShapes[i].MutateChromosome();
                }
            }

            for (int i = 0; i < _parentIndividual.SquareShapes.Count; i++)
            {
                _childIndividual.SquareShapes.Add(_parentIndividual.SquareShapes[i].CloneShapeChromosome());

                if (WillMutate())
                {
                    _childIndividual.SquareShapes[i].MutateChromosome();
                }
            }

            for (int i = 0; i < _parentIndividual.ElipseShapes.Count; i++)
            {
                _childIndividual.ElipseShapes.Add(_parentIndividual.ElipseShapes[i].CloneShapeChromosome());

                if (WillMutate())
                {
                    _childIndividual.ElipseShapes[i].MutateChromosome();
                }
            }

            for (int i = 0; i < _parentIndividual.PentagonShapes.Count; i++)
            {
                _childIndividual.PentagonShapes.Add(_parentIndividual.PentagonShapes[i].CloneShapeChromosome());

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
            else
            {
                AlgorithmInformation.KilledChilds++;
            }

            _childIndividual.CreateNewDNAString();

            return _childIndividual;
        }

        private bool WillMutate()
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
