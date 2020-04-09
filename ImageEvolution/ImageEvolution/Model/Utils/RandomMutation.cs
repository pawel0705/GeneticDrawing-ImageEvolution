using System;
using System.Collections.Generic;
using System.Text;

namespace ImageEvolution.Model.Settings
{
    public static class RandomMutation
    {
        private static readonly Random _random = new Random();

        public static byte RandomColour(byte min, byte max)
        {
            if(min > max)
            {
                var tmp = min;
                min = max;
                max = tmp;
            }

            return (byte)_random.Next(min, max);
        }

        public static int RandomPosition(int from, int to)
        {
            return _random.Next(from, to);
        }

        public static int RandomIntervalIntegerInclusive(int from, int to)
        {
            return _random.Next(from, to + 1);
        }

        public static int RandomIntegerNumber()
        {
            return _random.Next();
        }
    }
}
