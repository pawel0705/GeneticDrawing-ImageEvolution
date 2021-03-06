﻿using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Genetic.Evolution;
using ImageEvolution.Model.Settings;
using System.Collections.Generic;

using ImageEvolution.Model.Utils;

namespace ImageEvolution.Model.Genetic.Chromosome
{
    public class ShapeChromosome
    {
        public ColourDNA ColourShape { get; set; }

        public List<PositionDNA> PositionsShape { get; set; }

        private readonly ShapeType _shapeType = ShapeType.VOID;

        public ShapeChromosome(ShapeType shapeType)
        {
            _shapeType = shapeType;

            PositionsShape = new List<PositionDNA>();
            ColourShape = new ColourDNA();
        }

        public void InitializeDNA()
        {
            PositionsShape = new List<PositionDNA>();

            CreateVerticlePositions();

            ColourShape = new ColourDNA();
            ColourShape.InitializeDNA();
        }

        public ShapeChromosome CloneShapeChromosome()
        {
            var shape = new ShapeChromosome(_shapeType)
            {
                PositionsShape = new List<PositionDNA>(),
                ColourShape = (ColourDNA)ColourShape.CloneDNA()
            };

            foreach (var position in PositionsShape)
            {
                shape.PositionsShape.Add((PositionDNA)position.CloneDNA());
            }

            return shape;
        }

        private void CreateVerticlePositions()
        {
            if (_shapeType == ShapeType.TRIANGLE)
            {
                var pivotPoint = new PositionDNA();
                pivotPoint.InitializeDNA();
                PositionsShape.Add(pivotPoint);

                for (int i = 0; i < 2; i++)
                {
                    var position = new PositionDNA
                    {
                        PositionX = pivotPoint.PositionX,
                        PositionY = pivotPoint.PositionY
                    };

                    position.SoftMutation();

                    PositionsShape.Add(position);
                }
            }
            else if(_shapeType == ShapeType.SQUARE)
            {
                var position0 = new PositionDNA();
                position0.InitializeDNA();
                PositionsShape.Add(position0);

                var position1 = new PositionDNA
                {
                    PositionX = position0.PositionX,
                    PositionY = position0.PositionY
                };
                position1.SoftMutation();
                position1.PositionY = position0.PositionY;
                PositionsShape.Add(position1);

                var position2 = new PositionDNA
                {
                    PositionX = position1.PositionX,
                    PositionY = position1.PositionY
                };
                position2.SoftMutation();
                position2.PositionX = position1.PositionX;
                PositionsShape.Add(position2);

                var position3 = new PositionDNA
                {
                    PositionX = position0.PositionX,
                    PositionY = position2.PositionY
                };
                PositionsShape.Add(position3);
            }
            else if(_shapeType == ShapeType.CIRCLE)
            {
                var pivotPoint = new PositionDNA();
                pivotPoint.InitializeDNA();

                PositionsShape.Add(pivotPoint);

                var position = new PositionDNA
                {
                    PositionX = 10,
                    PositionY = 10
                };

                position.SoftMutation();

                PositionsShape.Add(position);
            }
            else if(_shapeType == ShapeType.PENTAGON)
            {
                var pivotPoint = new PositionDNA();
                pivotPoint.InitializeDNA();
                PositionsShape.Add(pivotPoint);

                for (int i = 0; i < 4; i++)
                {
                    var position = new PositionDNA
                    {
                        PositionX = pivotPoint.PositionX,
                        PositionY = pivotPoint.PositionY
                    };

                    position.SoftMutation();

                    PositionsShape.Add(position);
                }
            }

        }

        public void MutateChromosome()
        {
            switch (AlgorithmInformation.MutationType)
            {
                case MutationType.SOFT:
                    SoftMutation();
                    break;
                case MutationType.MEDIUM:
                    MediumMutation();
                    break;
                case MutationType.HARD:
                    HardMutation();
                    break;
                case MutationType.GAUSSIAN:
                    GaussianMutation();
                    break;
            }

        }

        private void MutateTriangleShapePosition()
        {
            var position = RandomMutation.RandomIntervalIntegerInclusive(0, PositionsShape.Count - 1);

            switch(AlgorithmInformation.MutationType)
            {
                case MutationType.SOFT:
                    PositionsShape[position].SoftMutation();
                    break;
                case MutationType.MEDIUM:
                    PositionsShape[position].MediumMutation();
                    break;
                case MutationType.HARD:
                    for (int i = 0; i < PositionsShape.Count; i++)
                    {
                        PositionsShape[i].HardMutation();
                    }
                    break;
                case MutationType.GAUSSIAN:
                    PositionsShape[position].GaussianMutation();
                    break;
            }
        }

        private void MutateSquareShapePosition()
        {
            var position = RandomMutation.RandomIntervalIntegerInclusive(0, PositionsShape.Count - 1);

            switch (AlgorithmInformation.MutationType)
            {
                case MutationType.SOFT:
                    PositionsShape[position].SoftMutation();
                    break;
                case MutationType.MEDIUM:
                    PositionsShape[position].MediumMutation();
                    break;
                case MutationType.GAUSSIAN:
                    PositionsShape[position].GaussianMutation();
                    break;
                case MutationType.HARD:
                    PositionsShape[0].HardMutation();
                    PositionsShape[1].PositionY = PositionsShape[0].PositionY;
                    PositionsShape[3].PositionX = PositionsShape[0].PositionX;

                    PositionsShape[2].HardMutation();
                    PositionsShape[1].PositionX = PositionsShape[2].PositionX;
                    PositionsShape[3].PositionY = PositionsShape[2].PositionY;
                    break;
            }

            if(AlgorithmInformation.MutationType != MutationType.HARD)
            {
                switch (position)
                {
                    case 0:
                        PositionsShape[1].PositionY = PositionsShape[position].PositionY;
                        PositionsShape[3].PositionX = PositionsShape[position].PositionX;
                        break;
                    case 1:
                        PositionsShape[0].PositionY = PositionsShape[position].PositionY;
                        PositionsShape[2].PositionX = PositionsShape[position].PositionX;
                        break;
                    case 2:
                        PositionsShape[1].PositionX = PositionsShape[position].PositionX;
                        PositionsShape[3].PositionY = PositionsShape[position].PositionY;
                        break;
                    case 3:
                        PositionsShape[0].PositionX = PositionsShape[position].PositionX;
                        PositionsShape[2].PositionY = PositionsShape[position].PositionY;
                        break;
                }
            }
        }

        private void MutateElipseShapePosition()
        {
            var position = RandomMutation.RandomIntervalIntegerInclusive(0, PositionsShape.Count - 1);

            switch (AlgorithmInformation.MutationType)
            {
                case MutationType.SOFT:
                    PositionsShape[position].SoftMutation();
                    break;
                case MutationType.MEDIUM:
                    PositionsShape[position].MediumMutation();
                    break;
                case MutationType.GAUSSIAN:
                    PositionsShape[position].GaussianMutation();
                    break;
                case MutationType.HARD:
                    for (int i = 0; i < PositionsShape.Count; i++)
                    {
                        PositionsShape[i].HardMutation();
                    }
                    break;
            }
        }

        private void MutatePentagonShapePosition()
        {
            var position = RandomMutation.RandomIntervalIntegerInclusive(0, PositionsShape.Count - 1);

            switch (AlgorithmInformation.MutationType)
            {
                case MutationType.SOFT:
                    PositionsShape[position].SoftMutation();
                    break;
                case MutationType.MEDIUM:
                    PositionsShape[position].MediumMutation();
                    break;
                case MutationType.GAUSSIAN:
                    PositionsShape[position].GaussianMutation();
                    break;
                case MutationType.HARD:
                    for (int i = 0; i < PositionsShape.Count; i++)
                    {
                        PositionsShape[i].HardMutation();
                    }
                    break;
            }
        }

        private void MutatePosition()
        {
            switch (_shapeType)
            {
                case ShapeType.TRIANGLE:
                    MutateTriangleShapePosition();
                    break;
                case ShapeType.SQUARE:
                    MutateSquareShapePosition();
                    break;
                case ShapeType.CIRCLE:
                    MutateElipseShapePosition();
                    break;
                case ShapeType.PENTAGON:
                    MutatePentagonShapePosition();
                    break;
            }

        }

        private void SoftMutation()
        {
            var colourOrPosition = RandomMutation.RandomIntervalIntegerInclusive(0, 1);

            switch(colourOrPosition)
            {
                case 0:
                    ColourShape.SoftMutation();
                    break;
                case 1:
                    MutatePosition();
                    break;
            }
        }

        private void MediumMutation()
        {
            var colourOrPosition = RandomMutation.RandomIntervalIntegerInclusive(0, 1);

            switch (colourOrPosition)
            {
                case 0:
                    ColourShape.MediumMutation();
                    break;
                case 1:
                    MutatePosition();
                    break;
            }
        }

        private void HardMutation()
        {
            ColourShape.HardMutation();
            MutatePosition();
        }

        private void GaussianMutation()
        {
            var colourOrPosition = RandomMutation.RandomIntervalIntegerInclusive(0, 1);

            switch (colourOrPosition)
            {
                case 0:
                    ColourShape.GaussianMutation();
                    break;
                case 1:
                    MutatePosition();
                    break;
            }
        }
    }
}
