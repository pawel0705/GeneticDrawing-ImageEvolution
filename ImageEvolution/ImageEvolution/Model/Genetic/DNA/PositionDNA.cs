using System;
using System.Collections.Generic;
using System.Text;

using ImageEvolution.Model.Settings;
using ImageEvolution.Model.Utils;

namespace ImageEvolution.Model.Genetic.DNA
{
    public class PositionDNA : IDNA
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public IDNA CloneDNA()
        {
            return new PositionDNA
            {
                PositionX = PositionX,
                PositionY = PositionY
            };
        }

        public void Initialize()
        {
            PositionX = RandomMutation.RandomPosition(0, AlgorithmSettings.ImageWidth);
            PositionY = RandomMutation.RandomPosition(0, AlgorithmSettings.ImageHeight);
        }
    }
}
