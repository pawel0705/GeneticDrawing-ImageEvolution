using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Documents;
using ImageEvolution.Model.Settings;
using ImageEvolution.Model.Utils;

namespace ImageEvolution.Model.Genetic.Evolution
{
    public class TwoParentEvolution : Evolution
    {
        public Individual[] _populationIndividuals { get; private set; }

        public Individual[] _populationToPass { get; set; }

        private Individual[] _eliteIndividuals;

        private List<Thread> _threads;

        private int[] _populationForThread;

        public void InitializeEvolution(Color[,] sourceIndividual)
        {
            if(AlgorithmInformation.Population < 4)
            {
                AlgorithmInformation.Population = 4;
            }

            if(AlgorithmInformation.Elite < 2)
            {
                AlgorithmInformation.Elite = 2;
            }

            _threads = new List<Thread>();
            _populationForThread = new int[AlgorithmInformation.GetOptimalThreadAmount()];

            _populationIndividuals = new Individual[AlgorithmInformation.Population];
            _populationToPass = new Individual[AlgorithmInformation.Population];
            _eliteIndividuals = new Individual[AlgorithmInformation.Elite];

            _destinationIndividual = sourceIndividual;

            _evolutionFitness = new EvolutionFitness(AlgorithmInformation.ImageWidth, AlgorithmInformation.ImageHeight);

            Individual individual;

            for (int i = 0; i < AlgorithmInformation.Population; i++)
            {
                individual = new Individual();
                individual.Initialize();

                _populationIndividuals[i] = individual;
            }

        }

        override public Individual Generate()
        {
            _generation++;

            if (AlgorithmInformation.DynamicMutation && (_generation % 100 == 0))
            {
                AlgorithmInformation.MutationChance -= 1;
            }

            var oneThread = AlgorithmInformation.Population / AlgorithmInformation.GetOptimalThreadAmount();
            if (oneThread * AlgorithmInformation.GetOptimalThreadAmount() != AlgorithmInformation.Population)
            {
                int delta = AlgorithmInformation.Population - (oneThread * (AlgorithmInformation.GetOptimalThreadAmount() - 1));

                for (int j = 0; j < AlgorithmInformation.GetOptimalThreadAmount() - 1; j++)
                {
                    _populationForThread[j] = oneThread;
                }

                _populationForThread[AlgorithmInformation.GetOptimalThreadAmount() - 1] = delta;
            }
            else
            {
                for (int j = 0; j < AlgorithmInformation.GetOptimalThreadAmount(); j++)
                {
                    _populationForThread[j] = oneThread;
                }

            }

            int from = 0;
            int to = _populationForThread[0];
            for (int j = 0; j < AlgorithmInformation.GetOptimalThreadAmount(); j++)
            {
                int[] coords = { from, to };

                var th = new Thread(() =>
                {
                    ComparePopulation(coords[0], coords[1]);
                });
                this._threads.Add(th);

                if ((j + 1) < _populationForThread.Length)
                {
                    from += _populationForThread[j];
                    to += _populationForThread[j + 1];
                }
            }

            foreach (Thread th in _threads)
            {
                th.Start();
            }

            foreach (Thread th in _threads)
            {
                th.Join();
            }




            _populationIndividuals = _populationIndividuals.OrderByDescending(i => i.Adaptation).ToArray();

            for (int j = 0; j < AlgorithmInformation.Population; j++)
            {
                _populationToPass[j] = _populationIndividuals[j].CloneIndividual();
            }

            for (int j = 0; j < AlgorithmInformation.Elite; j++)
            {
                _eliteIndividuals[j] = _populationIndividuals[j].CloneIndividual();
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

            OnIndividualCreated(_eliteIndividuals[0]);

            _threads.Clear();

            return _eliteIndividuals[0];
        }

        public IEnumerable<Individual> GetListOfIndividuals()
        {
            return _populationToPass;
        }

        private void ComparePopulation(int from, int to)
        {
            for (int i = from; i < to; i++)
            {
                lock (_populationIndividuals)
                {
                    _populationIndividuals[i].Generation = _generation;
                    _populationIndividuals[i].CreateNewDNAString();
                    _evolutionFitness.CompareImages(_populationIndividuals[i], _destinationIndividual);
                }
            }

            Console.WriteLine(from + " " + to);
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
    }
}
