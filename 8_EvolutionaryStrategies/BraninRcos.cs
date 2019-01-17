using System;

namespace _8_EvolutionaryStrategies
{
    public class BraninRcos
    {
        public double BraninRcosObjectiveFunction(double x1, double x2)
        {
            try
            {
                var result = Math.Pow((x2 - (5.1 / (4 * Math.Pow(Math.PI, 2))) * Math.Pow(x1, 2) + 5 / Math.PI * x1 - 6), 2) + (10 * (1 - 1 / (8 * Math.PI)) * Math.Cos(x1) + 10);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}