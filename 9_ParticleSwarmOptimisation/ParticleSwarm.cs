using System;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace _9_ParticleSwarmOptimisation
{
    public class ParticleSwarm
    {
        public void RunPso()
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

        private Particle[] CreateSwarmOfSolutions()
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
                                CurrentPosition = new[] { randomX1, randomX2 },
                                CurrentVelocity = new[] { initialVelocityX1, initialVelocityX2 },
                                PersonalBest = new[] { randomX1, randomX2 }, // first iteration = personal best = current position
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
                                swarm[particleNumber].CurrentPosition = new[] { randomX1, randomX2 };
                                swarm[particleNumber].CurrentVelocity = new[] { velocityX1, velocityX2 };
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
                            
                        }
                    }
                }
                return swarm;
            }
            catch (Exception exe)
            {
                Console.WriteLine(exe);
                throw;
            }
        }

        private double[] MoveToNewPostionWithInRange(double currentPositionOfX, double currentVelocityOfX, double personalBestOfX, double globalBestOfX, double min, double max)
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

                return new[] { newVelocityX, newRandomX };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private double CalculatePosition(double position, double newVelocity)
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

        private double CalculateVelocity(double position, double velocity, double personalBest, double globalBest)
        {
            try
            {
                var r1 = GetARandomNormarDistributionNumber(0, 1);      // random number from normal distribution between 0 to 1
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


        private void DisplaySwarm(Particle[] swarm)
        {
            try
            {
                Console.WriteLine("Particles of the swarm\n");
                foreach (var paritcle in swarm)
                {
                    Particle.DisplayParticle(paritcle);
                }
                Console.WriteLine("Best Particle of the swarm\n");
                var bestParticle = swarm.OrderBy(x => x.Cost).FirstOrDefault();
                Particle.DisplayParticle(bestParticle);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private double GetARandomNormarDistributionNumber(double mean, double stdDev)
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
}
