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
            PositionX = RandomMutation.RandomPosition(0, AlgorithmInformation.ImageWidth);
            PositionY = RandomMutation.RandomPosition(0, AlgorithmInformation.ImageHeight);
        }

        public IDNA CloneDNA()
        {
            return new PositionDNA()
            {
                PositionX = PositionX,
                PositionY = PositionY,
            };
        }

        public void SoftMutation()
        {

            var point = RandomMutation.RandomIntervalIntegerInclusive(0, 1);

            switch (point)
            {
                case 0:
                    PositionX = RandomMutation.RandomPosition(PositionX - AlgorithmInformation.SmallDeltaValue(), PositionX + AlgorithmInformation.SmallDeltaValue());
                    break;
                case 1:
                    PositionY = RandomMutation.RandomPosition(PositionY - AlgorithmInformation.SmallDeltaValue(), PositionY + AlgorithmInformation.SmallDeltaValue());
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
                    PositionX = RandomMutation.RandomPosition(0, AlgorithmInformation.ImageWidth);
                    break;
                case 1:
                    PositionY = RandomMutation.RandomPosition(0, AlgorithmInformation.ImageHeight);
                    break;
            }
        }

        public void HardMutation()
        {
            PositionX = RandomMutation.RandomPosition(0, AlgorithmInformation.ImageWidth);
            PositionY = RandomMutation.RandomPosition(0, AlgorithmInformation.ImageHeight);
        }

        public void GaussianMutation()
        {
            PositionX += RandomMutation.GaussianRandom(AlgorithmInformation.ImageWidth);
            PositionY += RandomMutation.GaussianRandom(AlgorithmInformation.ImageHeight);

            FixPosition();
        }


        private void FixPosition()
        {
            if (PositionX > AlgorithmInformation.ImageWidth)
            {
                PositionX = AlgorithmInformation.ImageWidth;
            }
            if (PositionX < 0)
            {
                PositionX = 0;
            }
            if (PositionY > AlgorithmInformation.ImageHeight)
            {
                PositionY = AlgorithmInformation.ImageHeight;
            }
            if (PositionY < 0)
            {
                PositionY = 0;
            }
        }
    }
}
