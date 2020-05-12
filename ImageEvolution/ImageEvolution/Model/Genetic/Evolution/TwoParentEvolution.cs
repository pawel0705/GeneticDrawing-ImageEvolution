using System;
using System.Drawing;
using System.Linq;
using ImageEvolution.Model.Settings;
using ImageEvolution.Model.Utils;

namespace ImageEvolution.Model.Genetic.Evolution
{
    public class TwoParentEvolution
    {
        private Individual[] _populationIndividuals;
        private Individual[] _eliteIndividuals;

        private Color[,] _destinationIndividual;

        private EvolutionFitness _evolutionFitness;

        private int _generation = 0;

        public void InitializeEvolution(Color[,] sourceIndividual)
        {
            _populationIndividuals = new Individual[AlgorithmInformation.Population];
            _eliteIndividuals = new Individual[AlgorithmInformation.Elite];

            _destinationIndividual = sourceIndividual;

            _evolutionFitness = new EvolutionFitness(Utils.AlgorithmInformation.ImageWidth, Utils.AlgorithmInformation.ImageHeight);

            Individual individual;

            for (int i = 0; i < AlgorithmInformation.Population; i++)
            {
                individual = new Individual();
                individual.Initialize();

                _populationIndividuals[i] = individual;
            }

        }

        public Individual Generate()
        {
            _generation++;

            if (AlgorithmInformation.DynamicMutation && (_generation % 100 == 0))
            {
                AlgorithmInformation.MutationChance -= 1;
            }

            for (int j = 0; j < AlgorithmInformation.Population; j++)
            {
                _populationIndividuals[j].Generation = _generation;
                _evolutionFitness.CompareImages(_populationIndividuals[j], _destinationIndividual);
            }

            Individual[] sortedGeneration = new Individual[AlgorithmInformation.Population];
            for (int j = 0; j < AlgorithmInformation.Population; j++)
            {
                sortedGeneration[j] = _populationIndividuals[j].CloneIndividual();
            }

            sortedGeneration = sortedGeneration.OrderByDescending(i => i.Adaptation).ToArray();

            for (int j = 0; j < AlgorithmInformation.Elite; j++)
            {
                _eliteIndividuals[j] = sortedGeneration[j];
            }

            int i = 0;
            int mother = 0;
            int father = 1;

            while (i < AlgorithmInformation.Population)
            {
                _populationIndividuals[i] = Reproduct(_eliteIndividuals[mother], _eliteIndividuals[father]);

                mother++;
                if (mother >= AlgorithmInformation.Elite)
                {
                    father++;
                    mother = 0;
                }

                if (father >= AlgorithmInformation.Elite)
                {
                    father = 0;
                }

                i++;
                if (i > AlgorithmInformation.Population)
                {
                    break;
                }
            }

            AlgorithmInformation.KilledChilds += AlgorithmInformation.Population - AlgorithmInformation.Elite;

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
                    individualChild.TriangleShapes.Add(mother.TriangleShapes[i].CloneShapeChromosome());
                }
                else
                {
                    individualChild.TriangleShapes.Add(father.TriangleShapes[i].CloneShapeChromosome());
                }

                if (WillMutate())
                {
                    individualChild.TriangleShapes[i].MutateChromosome();
                }
            }

            // square shapes
            for (int i = 0; i < mother.SquareShapes.Count; i++)
            {
                if (RandomMutation.RandomIntervalIntegerInclusive(0, 1) == 0)
                {
                    individualChild.SquareShapes.Add(mother.SquareShapes[i].CloneShapeChromosome());
                }
                else
                {
                    individualChild.SquareShapes.Add(father.SquareShapes[i].CloneShapeChromosome());
                }

                if (WillMutate())
                {
                    individualChild.SquareShapes[i].MutateChromosome();
                }
            }

            // elipse shapes
            for (int i = 0; i < mother.ElipseShapes.Count; i++)
            {
                if (RandomMutation.RandomIntervalIntegerInclusive(0, 1) == 0)
                {
                    individualChild.ElipseShapes.Add(mother.ElipseShapes[i].CloneShapeChromosome());
                }
                else
                {
                    individualChild.ElipseShapes.Add(father.ElipseShapes[i].CloneShapeChromosome());
                }

                if (WillMutate())
                {
                    individualChild.ElipseShapes[i].MutateChromosome();
                }
            }

            // pentagon shapes
            for (int i = 0; i < mother.PentagonShapes.Count; i++)
            {
                if (RandomMutation.RandomIntervalIntegerInclusive(0, 1) == 0)
                {
                    individualChild.PentagonShapes.Add(mother.PentagonShapes[i].CloneShapeChromosome());
                }
                else
                {
                    individualChild.PentagonShapes.Add(father.PentagonShapes[i].CloneShapeChromosome());
                }

                if (WillMutate())
                {
                    individualChild.PentagonShapes[i].MutateChromosome();
                }
            }

            return individualChild;
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
