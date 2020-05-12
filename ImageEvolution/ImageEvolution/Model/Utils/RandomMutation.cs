using System;

namespace ImageEvolution.Model.Settings
{
    public static class RandomMutation
    {
        private static readonly Random _random = new Random();

        public static byte RandomColour(byte min, byte max)
        {
            if (min > max)
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

        public static int GaussianRandom(int multipler)
        {
            double mean = 0, stdDev = 0.25;

            double u1 = 1.0 - _random.NextDouble();
            double u2 = 1.0 - _random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2);
            double randNormal = mean + stdDev * randStdNormal;

            if(randNormal < -1)
            {
                randNormal = 0;
            }
            else if(randNormal > 1)
            {
                randNormal = 0;
            }

            return (int)(randNormal * multipler);
        }
    }
}
