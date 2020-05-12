using ImageEvolution.Model.Genetic.Chromosome;
using ImageEvolution.Model.Utils;
using System.Collections.Generic;

namespace ImageEvolution.Model.Genetic.Evolution
{
    public class Individual
    {
        public double Adaptation { get; set; }

        public int Generation { get; set; }

        public List<ShapeChromosome> TriangleShapes { get; set; }
        public List<ShapeChromosome> SquareShapes { get; set; }
        public List<ShapeChromosome> ElipseShapes { get; set; }
        public List<ShapeChromosome> PentagonShapes { get; set; }

        public Individual()
        {
            TriangleShapes = new List<ShapeChromosome>();
            SquareShapes = new List<ShapeChromosome>();
            ElipseShapes = new List<ShapeChromosome>();
            PentagonShapes = new List<ShapeChromosome>();
        }

        public Individual CloneIndividual()
        {
            var individual = new Individual
            {
                TriangleShapes = new List<ShapeChromosome>(),
                SquareShapes = new List<ShapeChromosome>(),
                ElipseShapes = new List<ShapeChromosome>(),
                PentagonShapes = new List<ShapeChromosome>(),

                Adaptation = Adaptation,
                Generation = Generation
            };

            foreach (var shape in TriangleShapes)
            {
                individual.TriangleShapes.Add(shape.CloneShapeChromosome());
            }

            foreach (var shape in SquareShapes)
            {
                individual.SquareShapes.Add(shape.CloneShapeChromosome());
            }

            foreach (var shape in ElipseShapes)
            {
                individual.ElipseShapes.Add(shape.CloneShapeChromosome());
            }

            foreach (var shape in PentagonShapes)
            {
                individual.PentagonShapes.Add(shape.CloneShapeChromosome());
            }

            return individual;
        }

        public void Initialize()
        {
            int shapesAmount = AlgorithmInformation.ShapesAmount;

            for (int i = 0; i < shapesAmount;)
            {
                if(AlgorithmInformation.CircleShape)
                {
                    AddShape(ShapeType.CIRCLE);
                    i++;
                }

                if(AlgorithmInformation.PentagonShape)
                {
                    AddShape(ShapeType.PENTAGON);
                    i++;
                }

                if (AlgorithmInformation.SquareShape)
                {
                    AddShape(ShapeType.SQUARE);
                    i++;
                }

                if(AlgorithmInformation.TriangleShape)
                {
                    AddShape(ShapeType.TRIANGLE);
                    i++;
                }
            }
        }

        private void AddShape(ShapeType shapeType)
        {
            var shape = new ShapeChromosome(shapeType);
            shape.InitializeDNA();

            switch(shapeType)
            {
                case ShapeType.TRIANGLE:
                    TriangleShapes.Add(shape);
                    break;
                case ShapeType.SQUARE:
                    SquareShapes.Add(shape);
                    break;
                case ShapeType.CIRCLE:
                    ElipseShapes.Add(shape);
                    break;
                case ShapeType.PENTAGON:
                    PentagonShapes.Add(shape);
                    break;
            }
        }
    }
}