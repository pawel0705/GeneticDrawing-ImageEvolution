using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Utils
{
    public static class AlgorithmSettings
    {
        public static int ImageWidth { get; set; }
        public static int ImageHeight { get; set; }

        public static int ShapesAmount = 200;
        public static int Elite = 10;
        public static int Population = 50;

        public static int MutationChance = 10;
    }
}
