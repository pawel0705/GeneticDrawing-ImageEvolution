using ImageEvolution.Model.Genetic.Chromosome;
using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Genetic.Evolution
{
    public class Individual
    {
        public double Adaptation { get; set; }
        
        public int Generation { get; set; }

        public List<ShapeChromosome> Shapes { get; set; }

        public Individual()
        {
            Shapes = new List<ShapeChromosome>();
        }

        public Individual CloneIndividual()
        {
            var individual = new Individual();
            individual.Shapes = new List<ShapeChromosome>();
            individual.Adaptation = Adaptation;
            individual.Generation = Generation;

            foreach(var shape in Shapes)
            {
                individual.Shapes.Add(shape.CloneShapeChromosome());
            }

            return individual;
        }

        public void Initialize()
        {
            for (int i = 0; i < AlgorithmSettings.ShapesAmount; i++)
            {
                AddShape();
            }
        }

        private void AddShape()
        {
            var shape = new ShapeChromosome();
            shape.InitializeDNA();

            Shapes.Add(shape);
        }
    }
}
