using ImageEvolution.Model.Genetic.Evolution;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Utils
{
    public static class AlgorithmSettings
    {
        public static int ImageWidth { get; set; }
        public static int ImageHeight { get; set; }

        public static int ShapesAmount { get; set; }
        public static int Elite { get; set; }
        public static int Population { get; set; }

        public static int MutationChance { get; set; }

        public static bool CircleShape = false;
        public static bool TriangleShape = false;
        public static bool SquareShape = false;
        public static bool PentagonShape = false;

        public static MutationType MutationType { get; set; }
    }
}
