using System;

namespace _8_EvolutionaryStrategies
{
    public class RandomDouble
    {
        // https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers
        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}