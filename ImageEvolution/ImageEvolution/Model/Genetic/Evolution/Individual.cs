﻿using ImageEvolution.Model.Genetic.Chromosome;
using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace ImageEvolution.Model.Genetic.Evolution
{
    public class Individual
    {
        public double Adaptation { get; set; }
        
        public int Generation { get; set; }

        public List<ShapeChromosome> TriangleShapes { get; set; }
        public List<ShapeChromosome> SquareShapes { get; set; }

        public Individual()
        {
            TriangleShapes = new List<ShapeChromosome>();
            SquareShapes = new List<ShapeChromosome>();
        }

        public Individual CloneIndividual()
        {
            var individual = new Individual();
            individual.TriangleShapes = new List<ShapeChromosome>();
            individual.SquareShapes = new List<ShapeChromosome>();

            individual.Adaptation = Adaptation;
            individual.Generation = Generation;

            foreach(var shape in TriangleShapes)
            {
                individual.TriangleShapes.Add(shape.CloneShapeChromosome());
            }

            foreach (var shape in SquareShapes)
            {
                individual.SquareShapes.Add(shape.CloneShapeChromosome());
            }

            return individual;
        }

        public void Initialize()
        {
            int shapesAmount = AlgorithmSettings.ShapesAmount;

            for (int i = 0; i < shapesAmount;)
            {
                if(AlgorithmSettings.CircleShape)
                {
                    AddShape(ShapeType.CIRCLE);
                    i++;
                }

                if(AlgorithmSettings.PentagonShape)
                {
                    AddShape(ShapeType.PENTAGON);
                    i++;
                }

                if (AlgorithmSettings.SquareShape)
                {
                    AddShape(ShapeType.SQUARE);
                    i++;
                }

                if(AlgorithmSettings.TriangleShape)
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
            }
        }
    }
}