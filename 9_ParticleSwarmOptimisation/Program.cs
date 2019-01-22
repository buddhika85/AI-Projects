using System;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace _9_ParticleSwarmOptimisation
{
    /// <summary>
    /// Ref - https://stackoverflow.com/questions/10687016/what-is-e-in-floating-point 
    /// E notation in decimal numbers - 1.7E+3 = 1.7 x 10^3 = 1700
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Attempting to solve Branin Rcos Via Particle Swarm Optimisation");
                new ParticleSwarm().RunPso();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
