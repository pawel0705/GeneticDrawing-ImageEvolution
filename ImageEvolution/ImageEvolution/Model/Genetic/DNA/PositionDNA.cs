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

        public void InitializeDNA()
        {
            PositionX = RandomMutation.RandomPosition(0, AlgorithmSettings.ImageWidth);
            PositionY = RandomMutation.RandomPosition(0, AlgorithmSettings.ImageHeight);
        }

        public IDNA CloneDNA()
        {
            return new PositionDNA
            {
                PositionX = PositionX,
                PositionY = PositionY
            };
        }

        public void SoftMutation()
        {
            int smallDelta = 25;

            var point = RandomMutation.RandomIntervalIntegerInclusive(0, 1);

            switch (point)
            {
                case 0:
                    PositionX = RandomMutation.RandomPosition(PositionX - smallDelta, PositionX + smallDelta);
                    break;
                case 1:
                    PositionY = RandomMutation.RandomPosition(PositionY - smallDelta, PositionY + smallDelta);
                    break;
            }

            FixPosition();
        }

        public void MediumMutation()
        {
            var point = RandomMutation.RandomIntervalIntegerInclusive(0, 1);

            switch (point)
            {
                case 0:
                    PositionX = RandomMutation.RandomPosition(0, AlgorithmSettings.ImageWidth);
                    break;
                case 1:
                    PositionY = RandomMutation.RandomPosition(0, AlgorithmSettings.ImageHeight);
                    break;
            }
        }

        public void HardMutation()
        {
            PositionX = RandomMutation.RandomPosition(0, AlgorithmSettings.ImageWidth);
            PositionY = RandomMutation.RandomPosition(0, AlgorithmSettings.ImageHeight);
        }

        public void GaussianMutation()
        {
            throw new NotImplementedException();
        }


        private void FixPosition()
        {
            if (PositionX > AlgorithmSettings.ImageWidth)
            {
                PositionX = AlgorithmSettings.ImageWidth;
            }
            if (PositionX < 0)
            {
                PositionX = 0;
            }
            if (PositionY > AlgorithmSettings.ImageHeight)
            {
                PositionY = AlgorithmSettings.ImageHeight;
            }
            if (PositionY < 0)
            {
                PositionY = 0;
            }
        }
    }
}
