using System;

namespace _9_ParticleSwarmOptimisation
{
    public class Particle
    {
        public int ParticleId { get; set; }
        public double[] CurrentPosition { get; set; }
        public double[] CurrentVelocity { get; set; }
        public double[] PersonalBest { get; set; }
        public double Cost { get; set; }                // result of Branin Rcos

        public static double[] GlobalBestPosition = { double.MaxValue, double.MaxValue };       // its a minimisation probelm - and initialy worst possible values are stored
        public static double GlobalBestCost = double.MaxValue;

        public static void DisplayParticle(Particle paritcle, bool isBest)
        {
            try
            {
                Console.ForegroundColor = isBest ? ConsoleColor.Green : ConsoleColor.White;
                var braninRcos = new BraninRcos();
                Console.WriteLine(
                    "Particle Number = {0}\n x1 = {1}, x2 = {2}, Result = {3} " +
                    "\nPersonal Best = [{4}, {5}], Personal Best Result = {6} " +
                    "\nGlobal Best = [{7}, {8}], Global Best Result = {9}\n",
                    paritcle.ParticleId, paritcle.CurrentPosition[0], paritcle.CurrentPosition[1], paritcle.Cost,
                    paritcle.PersonalBest[0], paritcle.PersonalBest[1],
                    braninRcos.BraninRcosObjectiveFunction(paritcle.PersonalBest[0], paritcle.PersonalBest[1]),
                    Particle.GlobalBestPosition[0], Particle.GlobalBestPosition[1], Particle.GlobalBestCost);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}