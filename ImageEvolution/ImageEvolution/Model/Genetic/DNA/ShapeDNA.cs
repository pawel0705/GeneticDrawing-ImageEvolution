using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Genetic.DNA
{
    public class ShapeDNA : IDNA
    {
        public List<PositionDNA> PositionsShape { get; set; }
        public ColourDNA ColourShape { get; set; }
        public ShapeType shapeType = ShapeType.VOID;

        public void Initialize()
        {
            PositionsShape = new List<PositionDNA>();
            shapeType = ShapeType.TRIANGLE;

            CreateVerticlePositions();

            ColourShape = new ColourDNA();
            ColourShape.Initialize();
        }

        private void CreateVerticlePositions()
        {
            switch(shapeType)
            {
                case ShapeType.TRIANGLE:
                    for(int i = 0; i < 3; i++)
                    {
                        var position = new PositionDNA();
                        position.Initialize();

                        PositionsShape.Add(position);
                    }
                    break;
            }

        }

        public IDNA CloneDNA()
        {
            var shape = new ShapeDNA();
            shape.PositionsShape = new List<PositionDNA>();
            shape.ColourShape = (ColourDNA)ColourShape.CloneDNA();

            foreach(var position in PositionsShape)
            {
                shape.PositionsShape.Add((PositionDNA)position.CloneDNA());
            }

            return shape;
        }
    }
}
