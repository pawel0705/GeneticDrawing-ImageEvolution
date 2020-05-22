using ImageEvolution.Model.Genetic.Chromosome;
using ImageEvolution.Model.Utils;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Genetic.Evolution
{
    public class Individual
    {
        public double Adaptation { get; set; }

        public int Generation { get; set; }

        public StringBuilder DNAstring { get; set; }

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

            DNAstring = new StringBuilder();
        }

        public void CreateNewDNAString()
        {
            DNAstring.Clear();

            DNAstring.Append("T");
            foreach(var triangle in TriangleShapes)
            {
                DNAstring.Append(triangle.ColourShape.RedColour + " " + triangle.ColourShape.GreenColour + " " + triangle.ColourShape.BlueColour + " " + triangle.ColourShape.AlphaColour + " ");
                DNAstring.Append(triangle.PositionsShape[0].PositionX + " " + triangle.PositionsShape[0].PositionY + " " +
                    triangle.PositionsShape[1].PositionX + " " + triangle.PositionsShape[1].PositionY + " " +
                    triangle.PositionsShape[2].PositionX + " " + triangle.PositionsShape[2].PositionY + " ");
            }

            DNAstring.Append("S");
            foreach(var square in SquareShapes)
            {
                DNAstring.Append(square.ColourShape.RedColour + " " + square.ColourShape.GreenColour + " " + square.ColourShape.BlueColour + " " + square.ColourShape.AlphaColour + " ");
                DNAstring.Append(square.PositionsShape[0].PositionX + " " + square.PositionsShape[0].PositionY + " " +
                    square.PositionsShape[1].PositionX + " " + square.PositionsShape[1].PositionY + " " +
                    square.PositionsShape[2].PositionX + " " + square.PositionsShape[2].PositionY + " " +
                    square.PositionsShape[3].PositionX + " " + square.PositionsShape[3].PositionY + " ");
            }

            DNAstring.Append("E");
            foreach (var elipse in ElipseShapes)
            {
                DNAstring.Append(elipse.ColourShape.RedColour + " " + elipse.ColourShape.GreenColour + " " + elipse.ColourShape.BlueColour + " " + elipse.ColourShape.AlphaColour + " ");
                DNAstring.Append(elipse.PositionsShape[0].PositionX + " " + elipse.PositionsShape[0].PositionY + " " +
                    elipse.PositionsShape[1].PositionX + " " + elipse.PositionsShape[1].PositionY + " ");
            }

            DNAstring.Append("P");
            foreach (var pentagon in PentagonShapes)
            {
                DNAstring.Append(pentagon.ColourShape.RedColour + " " + pentagon.ColourShape.GreenColour + " " + pentagon.ColourShape.BlueColour + " " + pentagon.ColourShape.AlphaColour + " ");
                DNAstring.Append(pentagon.PositionsShape[0].PositionX + " " + pentagon.PositionsShape[0].PositionY + " " +
                    pentagon.PositionsShape[1].PositionX + " " + pentagon.PositionsShape[1].PositionY + " " +
                    pentagon.PositionsShape[2].PositionX + " " + pentagon.PositionsShape[2].PositionY + " " +
                    pentagon.PositionsShape[3].PositionX + " " + pentagon.PositionsShape[3].PositionY + " " +
                    pentagon.PositionsShape[4].PositionX + " " + pentagon.PositionsShape[4].PositionY + " ");
            }
        }

        public Individual CloneIndividual()
        {
            var individual = new Individual
            {
                TriangleShapes = new List<ShapeChromosome>(),
                SquareShapes = new List<ShapeChromosome>(),
                ElipseShapes = new List<ShapeChromosome>(),
                PentagonShapes = new List<ShapeChromosome>(),
                DNAstring = new StringBuilder(),

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

            individual.DNAstring.Append(DNAstring);

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