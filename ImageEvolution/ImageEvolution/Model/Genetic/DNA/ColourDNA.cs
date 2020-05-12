using System;

using ImageEvolution.Model.Settings;
using ImageEvolution.Model.Utils;

namespace ImageEvolution.Model.Genetic.DNA
{
    public class ColourDNA : IDNA
    {
        public byte RedColour { get; set; }
        public byte GreenColour { get; set; }
        public byte BlueColour { get; set; }
        public byte AlphaColour { get; set; }

        public IDNA CloneDNA()
        {
            return new ColourDNA
            {
                RedColour = RedColour,
                GreenColour = GreenColour,
                BlueColour = BlueColour,
                AlphaColour = AlphaColour
            };
        }

        public void InitializeDNA()
        {
            RedColour = 0;
            GreenColour = 0;
            BlueColour = 0;
            AlphaColour = 0;
        }

        public void SoftMutation()
        {
            var colourMutation = RandomMutation.RandomIntervalIntegerInclusive(0, 4);

            switch (colourMutation)
            {
                case 0:
                    RedColour = RandomMutation.RandomColour((byte)(RedColour - AlgorithmInformation.SmallDeltaValue()), (byte)(RedColour + AlgorithmInformation.SmallDeltaValue()));
                    break;
                case 1:
                    GreenColour = RandomMutation.RandomColour((byte)(GreenColour - AlgorithmInformation.SmallDeltaValue()), (byte)(GreenColour + AlgorithmInformation.SmallDeltaValue()));
                    break;
                case 2:
                    BlueColour = RandomMutation.RandomColour((byte)(BlueColour - AlgorithmInformation.SmallDeltaValue()), (byte)(BlueColour + AlgorithmInformation.SmallDeltaValue()));
                    break;
                case 3:
                    AlphaColour = RandomMutation.RandomColour((byte)(BlueColour - AlgorithmInformation.SmallDeltaValue()), (byte)(BlueColour + AlgorithmInformation.SmallDeltaValue()));
                    break;
            }

        }

        public void MediumMutation()
        {
            var colourMutation = RandomMutation.RandomIntervalIntegerInclusive(0, 4);

            switch (colourMutation)
            {
                case 0:
                    RedColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
                    break;
                case 1:
                    GreenColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
                    break;
                case 2:
                    BlueColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
                    break;
                case 3:
                    AlphaColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
                    break;
            }
        }

        public void HardMutation()
        {
            RedColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
            GreenColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
            BlueColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
            AlphaColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
        }

        public void GaussianMutation()
        {
            var colourMutation = RandomMutation.RandomIntervalIntegerInclusive(0, 4);

            switch (colourMutation)
            {
                case 1:
                    RedColour += (byte)RandomMutation.GaussianRandom(Byte.MaxValue);
                    break;
                case 2:
                    GreenColour += (byte)RandomMutation.GaussianRandom(Byte.MaxValue);
                    break;
                case 3:
                    BlueColour += (byte)RandomMutation.GaussianRandom(Byte.MaxValue);
                    break;
                case 4:
                    AlphaColour += (byte)RandomMutation.GaussianRandom(Byte.MaxValue);
                    break;
            }
        }
    }
}
