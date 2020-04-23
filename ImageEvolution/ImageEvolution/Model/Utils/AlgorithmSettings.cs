using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Utils
{
    public static class AlgorithmSettings
    {
        public static int ImageWidth { get; set; }
        public static int ImageHeight { get; set; }

        public static int ShapesAmount = 150;
        public static int Elite = 5;
        public static int Population = 40;

        public static int MutationChance = 10;

        public static bool CircleShape = false;
        public static bool TriangleShape = false;
        public static bool SquareShape = false;
        public static bool PentagonShape = false;
    }
}
