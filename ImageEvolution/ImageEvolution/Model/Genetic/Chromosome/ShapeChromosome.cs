using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Genetic.Evolution;
using ImageEvolution.Model.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace ImageEvolution.Model.Genetic.Chromosome
{
    public class ShapeChromosome
    {
        public ColourDNA ColourShape { get; set; }

        public List<PositionDNA> PositionsShape { get; set; }

        private ShapeType _shapeType = ShapeType.VOID;

        public ShapeChromosome(ShapeType shapeType)
        {
            _shapeType = shapeType;
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
            var shape = new ShapeChromosome(_shapeType);

            shape.PositionsShape = new List<PositionDNA>();
            shape.ColourShape = (ColourDNA)ColourShape.CloneDNA();

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
                    var position = new PositionDNA();
                    position.PositionX = pivotPoint.PositionX;
                    position.PositionY = pivotPoint.PositionY;

                    position.SoftMutation();

                    PositionsShape.Add(position);
                }
            }
            else if(_shapeType == ShapeType.SQUARE)
            {
                var position0 = new PositionDNA();
                position0.InitializeDNA();
                PositionsShape.Add(position0);

                var position1 = new PositionDNA();
                position1.PositionX = position0.PositionX;
                position1.PositionY = position0.PositionY;
                position1.SoftMutation();
                position1.PositionY = position0.PositionY;
                PositionsShape.Add(position1);

                var position2 = new PositionDNA();
                position2.PositionX = position1.PositionX;
                position2.PositionY = position1.PositionY;
                position2.SoftMutation();
                position2.PositionX = position1.PositionX;
                PositionsShape.Add(position2);

                var position3 = new PositionDNA();
                position3.PositionX = position0.PositionX;
                position3.PositionY = position2.PositionY;
                PositionsShape.Add(position3);
            }
            else if(_shapeType == ShapeType.CIRCLE)
            {
                var pivotPoint = new PositionDNA();
                pivotPoint.InitializeDNA();

                PositionsShape.Add(pivotPoint);

                var position = new PositionDNA();
                position.PositionX = 10;
                position.PositionY = 10;

                position.SoftMutation();

                PositionsShape.Add(position);
            }
        }

        public void MutateChromosome(MutationType mutationType)
        {
            switch (mutationType)
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
            PositionsShape[position].SoftMutation();
        }

        private void MutateSquareShapePosition()
        {
            var position = RandomMutation.RandomIntervalIntegerInclusive(0, PositionsShape.Count - 1);

            PositionsShape[position].SoftMutation();

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

        private void MutateElipseShapePosition()
        {
            var positionSize = RandomMutation.RandomIntervalIntegerInclusive(0, PositionsShape.Count - 1);

            PositionsShape[positionSize].SoftMutation();
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
                    var position = RandomMutation.RandomIntervalIntegerInclusive(0, PositionsShape.Count - 1);
                    PositionsShape[position].MediumMutation();
                    break;
            }
        }

        private void HardMutation()
        {
            ColourShape.HardMutation();

            for(int i = 0; i < PositionsShape.Count; i++)
            {
                PositionsShape[i].HardMutation();
            }
        }

        private void GaussianMutation()
        {

        }
    }
}
