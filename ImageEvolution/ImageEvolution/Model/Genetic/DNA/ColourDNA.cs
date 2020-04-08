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

        public void Initialize()
        {
            RedColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
            GreenColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
            BlueColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
            AlphaColour = RandomMutation.RandomColour(Byte.MinValue, Byte.MaxValue);
        }
    }
}
