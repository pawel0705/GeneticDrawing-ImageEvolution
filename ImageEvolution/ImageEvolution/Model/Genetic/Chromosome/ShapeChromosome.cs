using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Genetic.Evolution;
using ImageEvolution.Model.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Genetic.Chromosome
{
    public class ShapeChromosome
    {
        public ColourDNA ColourShape { get; set; }
        public List<PositionDNA> PositionsShape { get; set; }

        private ShapeType shapeType = ShapeType.VOID;

        public void InitializeDNA()
        {
            PositionsShape = new List<PositionDNA>();
            shapeType = ShapeType.TRIANGLE;

            CreateVerticlePositions();

            ColourShape = new ColourDNA();
            ColourShape.InitializeDNA();
        }

        public ShapeChromosome CloneShapeChromosome()
        {
            var shape = new ShapeChromosome();

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
            switch (shapeType)
            {
                case ShapeType.TRIANGLE:
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
                    break;
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

        private void SoftMutation()
        {
            var colourOrPosition = RandomMutation.RandomIntervalIntegerInclusive(0, 1);

            switch(colourOrPosition)
            {
                case 0:
                    ColourShape.SoftMutation();
                    break;
                case 1:
                    var position = RandomMutation.RandomIntervalIntegerInclusive(0, PositionsShape.Count - 1);
                    PositionsShape[position].SoftMutation();
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
