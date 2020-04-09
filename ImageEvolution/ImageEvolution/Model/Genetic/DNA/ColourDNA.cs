using System;
using System.Collections.Generic;
using System.Text;

using ImageEvolution.Model.Settings;

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
            RedColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
            GreenColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
            BlueColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
            AlphaColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
        }

        public void SoftMutation()
        {
            var colourMutation = RandomMutation.RandomIntervalIntegerInclusive(0, 4);

            byte deltaValue = 25;


            switch (colourMutation)
            {
                case 0:
                    RedColour = RandomMutation.RandomColour((byte)(RedColour - deltaValue), (byte)(RedColour + deltaValue));
                    break;
                case 1:
                    GreenColour = RandomMutation.RandomColour((byte)(GreenColour - deltaValue), (byte)(GreenColour + deltaValue));
                    break;
                case 2:
                    BlueColour = RandomMutation.RandomColour((byte)(BlueColour - deltaValue), (byte)(BlueColour + deltaValue));
                    break;
                case 3:
                    AlphaColour = RandomMutation.RandomColour((byte)(BlueColour - deltaValue), (byte)(BlueColour + deltaValue));
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
            throw new NotImplementedException();
        }
    }
}
