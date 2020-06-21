using ImageEvolution.Model.Genetic.Chromosome;
using ImageEvolution.Model.Genetic.DNA;
using ImageEvolution.Model.Utils;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

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

            DNAstring.Append("XX");
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

        public void InitializeFromDNA(IndividualListData individualData)
        {
            this.Adaptation = Convert.ToDouble(individualData.Fitness);

            this.DNAstring.Append(individualData.IndividualDNA);

            ShapeChromosome shapeChromosome;

            string informationBuffer = "";

            PositionDNA positionDNA;

            for (int i = 0; i < DNAstring.Length;)
            {

                if (DNAstring[i] == 'T')
                {
                    i++;

                    while(DNAstring[i] != 'S')
                    {
                        shapeChromosome = new ShapeChromosome(ShapeType.TRIANGLE);

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        shapeChromosome.ColourShape.RedColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.GreenColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.BlueColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.AlphaColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                      
                        positionDNA = new PositionDNA(); // 1

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        positionDNA = new PositionDNA(); // 2

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        positionDNA = new PositionDNA(); // 3

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        TriangleShapes.Add(shapeChromosome); // TRIANGLE
                    }
                } 
                else if (DNAstring[i] == 'S')
                {
                    i++;

                    while (DNAstring[i] != 'E')
                    {
                        shapeChromosome = new ShapeChromosome(ShapeType.SQUARE);

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        shapeChromosome.ColourShape.RedColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.GreenColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.BlueColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.AlphaColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;


                        positionDNA = new PositionDNA(); // 1

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        positionDNA = new PositionDNA(); // 2

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        positionDNA = new PositionDNA(); // 3

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        positionDNA = new PositionDNA(); // 4

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        SquareShapes.Add(shapeChromosome); // SQUARE
                    }
                }
                else if(DNAstring[i] == 'E')
                {
                    i++;

                    while (DNAstring[i] != 'P')
                    {
                        shapeChromosome = new ShapeChromosome(ShapeType.CIRCLE);

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        shapeChromosome.ColourShape.RedColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.GreenColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.BlueColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.AlphaColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;


                        positionDNA = new PositionDNA(); // 1

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        positionDNA = new PositionDNA(); // 2

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        ElipseShapes.Add(shapeChromosome); // ELIPSE
                    }
                }
                else if(DNAstring[i] == 'P')
                {
                    i++;

                    while (DNAstring[i] != 'X')
                    {
                        shapeChromosome = new ShapeChromosome(ShapeType.PENTAGON);

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        shapeChromosome.ColourShape.RedColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.GreenColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.BlueColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        shapeChromosome.ColourShape.AlphaColour = (byte)Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;


                        positionDNA = new PositionDNA(); // 1

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        positionDNA = new PositionDNA(); // 2

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        positionDNA = new PositionDNA(); // 3

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        positionDNA = new PositionDNA(); // 4

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        positionDNA = new PositionDNA(); // 5

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }


                        positionDNA.PositionX = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;

                        while (DNAstring[i] != ' ')
                        {
                            informationBuffer += DNAstring[i];
                            i++;
                        }

                        positionDNA.PositionY = Int32.Parse(informationBuffer);
                        informationBuffer = "";
                        i++;
                        shapeChromosome.PositionsShape.Add(positionDNA);

                        PentagonShapes.Add(shapeChromosome); // PENTAGON
                    }
                }
                else if(DNAstring[i] == 'X')
                {
                    return;
                }
                else
                {
                    i++;
                }
            }
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