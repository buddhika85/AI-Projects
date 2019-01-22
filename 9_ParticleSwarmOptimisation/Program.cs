using System;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace _9_ParticleSwarmOptimisation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Attempting to solve Branin Rcos Via Particle Swarm Optimisation");
                RunPso();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void RunPso()
        {
            try
            {
                Particle[] swarm = CreateSwarmOfSolutions();
                DisplaySwarm(swarm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void DisplaySwarm(Particle[] swarm)
        {
            try
            {
                Console.WriteLine("Particles of the swarm\n");
                foreach (var paritcle in swarm)
                {
                    DisplayParticle(paritcle);
                }
                Console.WriteLine("Best Particle of the swarm\n");
                var bestParticle = swarm.OrderBy(x => x.Cost).FirstOrDefault();
                DisplayParticle(bestParticle);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void DisplayParticle(Particle paritcle)
        {
            try
            {
                var braninRcos = new BraninRcos();
                Console.WriteLine(
                    "Particle Number = {0}\n x1 = {1}, x2 = {2}, Result = {3} " +
                    "\nPersonal Best = [{4}, {5}], Personal Best Result = {6} " +
                    "\nGlobal Best = [{7}, {8}], Globast Best Result = {9}\n",
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

        private static Particle[] CreateSwarmOfSolutions()
        {
            try
            {
                var swarm = new Particle[Config.SwarmSize];
                var radomDouble = new RandomDouble();
                var braninRcos = new BraninRcos();
                for (var particleIterationNumber = 0;
                    particleIterationNumber < Config.NumberOfIterations;
                    particleIterationNumber++)
                {
                    // step 1 - create initial swarm of particles
                    if (particleIterationNumber == 0)
                    {
                        for (var particleNumber = 0; particleNumber < swarm.Length; particleNumber++)
                        {
                            double randomX1;
                            double randomX2;
                            do
                            {
                                randomX1 = radomDouble.GetRandomNumber(Config.MinX1, Config.MaxX1);
                                randomX2 = radomDouble.GetRandomNumber(Config.MinX2, Config.MaxX2);
                            } while (particleNumber != 0 &&
                                     Math.Abs(randomX1 - swarm[particleNumber - 1].CurrentPosition[0]) < 0.000000000000001 &&
                                     Math.Abs(randomX2 - swarm[particleNumber - 1].CurrentPosition[1]) < 0.000000000000001);

                           
                            var initialVelocityX1 = 0.0; // first iteration = velocity = 0.0
                            var initialVelocityX2 = 0.0; // first iteration = velocity = 0.0
                            var currentCost = braninRcos.BraninRcosObjectiveFunction(randomX1, randomX2);
                            var particleOfFirstIteration = new Particle
                            {
                                ParticleId = particleNumber + 1,
                                CurrentPosition = new[] {randomX1, randomX2},
                                CurrentVelocity = new[] {initialVelocityX1, initialVelocityX2},
                                PersonalBest = new[] {randomX1, randomX2}, // first iteration = personal best = current position
                                Cost = currentCost
                            };

                            // update global best if paricule performs better than previouse global best
                            if (currentCost < Particle.GlobalBestCost)
                            {
                                Particle.GlobalBestCost = currentCost;
                                Particle.GlobalBestPosition = particleOfFirstIteration.CurrentPosition;
                            }
                            swarm[particleNumber] = particleOfFirstIteration;
                        }
                    }
                    else
                    {
                        // 2nd iteration onwards
                        for (var particleNumber = 0; particleNumber < swarm.Length; particleNumber++)
                        {
                            var currentParticlesCost = braninRcos.BraninRcosObjectiveFunction(
                                swarm[particleNumber].CurrentPosition[0], swarm[particleNumber].CurrentPosition[1]);

                            // move to a new position if the current particle is not global best solution found so far
                            if (currentParticlesCost > Particle.GlobalBestCost)
                            {

                                var newVelocityAndPosition = MoveToNewPostionWithInRange(swarm[particleNumber].CurrentPosition[0],
                                    swarm[particleNumber].CurrentVelocity[0], swarm[particleNumber].PersonalBest[0],
                                    Particle.GlobalBestPosition[0], Config.MinX1, Config.MaxX1);
                                var velocityX1 = newVelocityAndPosition[0];
                                var randomX1 = newVelocityAndPosition[1];

                                newVelocityAndPosition = MoveToNewPostionWithInRange(swarm[particleNumber].CurrentPosition[1],
                                    swarm[particleNumber].CurrentVelocity[1], swarm[particleNumber].PersonalBest[1],
                                    Particle.GlobalBestPosition[1], Config.MinX2, Config.MaxX2);
                                var velocityX2 = newVelocityAndPosition[0];
                                var randomX2 = newVelocityAndPosition[1];

                                //var velocityX2 = CalculateVelocity(swarm[particleNumber].CurrentPosition[1],
                                //    swarm[particleNumber].CurrentVelocity[1], swarm[particleNumber].PersonalBest[0],
                                //    Particle.GlobalBestPosition[0]);
                                //var randomX2 = CalculatePosition(swarm[particleNumber].CurrentPosition[1], velocityX2);

                                var currentCost = braninRcos.BraninRcosObjectiveFunction(randomX1, randomX2);

                                // update particle
                                swarm[particleNumber].CurrentPosition = new[] {randomX1, randomX2};
                                swarm[particleNumber].CurrentVelocity = new[] {velocityX1, velocityX2};
                                swarm[particleNumber].Cost = currentCost;
                                
                                // update personal best 
                                var personalBestCost = braninRcos.BraninRcosObjectiveFunction(
                                    swarm[particleNumber].PersonalBest[0], swarm[particleNumber].PersonalBest[1]);
                                if (currentCost < personalBestCost)
                                {
                                    swarm[particleNumber].PersonalBest = new[] { randomX1, randomX2 };
                                }

                                // update global best if paricule performs better than previouse global best
                                if (currentCost < Particle.GlobalBestCost)
                                {
                                    Particle.GlobalBestCost = currentCost;
                                    Particle.GlobalBestPosition = swarm[particleNumber].CurrentPosition;
                                }
                            }
                            else
                            {
                                Particle.GlobalBestCost = currentParticlesCost;
                                Particle.GlobalBestPosition = swarm[particleNumber].CurrentPosition;
                            }
                        }
                    }

                    //Console.WriteLine("Iteration {0} complete \n", particleIterationNumber + 1);
                }
                return swarm;
            }
            catch (Exception exe)
            {
                Console.WriteLine(exe);
                throw;
            }
        }

        private static double[] MoveToNewPostionWithInRange(double currentPositionOfX, double currentVelocityOfX, double personalBestOfX, double globalBestOfX, double min, double max)
        {
            try
            {
                double newVelocityX;
                double newRandomX;
                //do
                //{
                    newVelocityX = CalculateVelocity(currentPositionOfX, currentVelocityOfX, personalBestOfX, globalBestOfX);
                    newRandomX = CalculatePosition(currentPositionOfX, newVelocityX);
                //} while (newRandomX < min || newRandomX > max);

                return new[] {newVelocityX, newRandomX};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static double CalculatePosition(double position, double newVelocity)
        {
            try
            {
                var newPosition = position + newVelocity;
                return newPosition;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static double CalculateVelocity(double position, double velocity, double personalBest, double globalBest)
        {
            try
            {
                var r1 =  GetARandomNormarDistributionNumber(0, 1);      // random number from normal distribution between 0 to 1
                var r2 = GetARandomNormarDistributionNumber(0, 1);      // random number from normal distribution between 0 to 1
                var newVelocity = Config.WInertiaWeight * velocity +
                                  r1 * Config.C1CognitiveLocalWeight * (personalBest - position) +
                                  r2 * Config.C2SocialGlobalWeight * (globalBest - position);
                return newVelocity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static double GetARandomNormarDistributionNumber(double mean, double stdDev)
        {
            try
            {
                Normal normalDist = new Normal(mean, stdDev);
                double randomGaussianValue = normalDist.Sample();
                return randomGaussianValue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class Particle
    {
        public int ParticleId { get; set; }
        public double[] CurrentPosition { get; set; }
        public double[] CurrentVelocity { get; set; }
        public double[] PersonalBest { get; set; }
        public double Cost { get; set; }                // result of Branin Rcos

        public static double[] GlobalBestPosition = { double.MaxValue, double.MaxValue };       // its a minimisation probelm - and initialy worst possible values are stored
        public static double GlobalBestCost = double.MaxValue;
    }

    public static class Config
    {
        public static int SwarmSize = 5;
        public static int NumberOfIterations = 100;
        
        public static double MinX1 = -5;
        public static double MaxX1 = 10;
        public static double MinX2 = 0;
        public static double MaxX2 = 15;

        public static double WInertiaWeight = 0.729;
        public static double C1CognitiveLocalWeight = 1.49445;  // Personal acceleration coefficient
        public static double C2SocialGlobalWeight = 1.49445;    // Social acceleration coefficient 
    }

    public class RandomDouble
    {
        // https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers
        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }

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
