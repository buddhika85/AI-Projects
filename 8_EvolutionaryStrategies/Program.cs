using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace _8_EvolutionaryStrategies
{
    class Program
    {
       
        static void Main(string[] args)
        {
            var generations = new List<GenerationDetails>();

            var braninRcos = new BraninRcos();
            var initialOutput = braninRcos.BraninRcosObjectiveFunction(Config.InitialX1, Config.InitialX2);
            Console.WriteLine("\nInitial Test Solution");
            DisplayChromosome(Config.InitialX1, Config.InitialSigmaX1, Config.InitialX2, Config.InitialSigmaX2, initialOutput);

            ArtificialChromosome [] initialPopulation = GenerateInitialPopulation();
            Console.WriteLine("\nInitial Population");
            foreach (var solution in initialPopulation)
            {
                DisplayChromosome(solution);
            }

            var bestChromosome = GetBestSolution(initialPopulation);
            Console.WriteLine("\nBest Solution Initial Population");
            DisplayChromosome(bestChromosome);
            generations.Add(new GenerationDetails
            {
                Number = 1,
                BastOfGeneration = bestChromosome
            });

            ArtificialChromosome[] parentsForNextGeneration = null;
            for (int i = 2; i <= Config.NumberOfGenerationsM; i++)
            {
                if (i == 2)
                {
                    parentsForNextGeneration = CreateNextGeneration(initialPopulation);
                }
                else
                {
                    parentsForNextGeneration = CreateNextGeneration(parentsForNextGeneration);
                }

                bestChromosome = GetBestSolution(parentsForNextGeneration);
                Console.WriteLine("\nBest Solution of Generation {0} - ", i);
                DisplayChromosome(bestChromosome);

                generations.Add(new GenerationDetails
                {
                    Number = i,
                    BastOfGeneration = bestChromosome
                });
            }

            //foreach (var generation in generations)
            //{
            //    Console.Write("Generation {0} Best Solution ", generation.Number);
            //    DisplayChromosome(generation.BastOfGeneration);
            //}

            Console.ReadKey();
        }

        private static ArtificialChromosome[] CreateNextGeneration(ArtificialChromosome[] parentPopulation)
        {
            try
            {
                ArtificialChromosome[] children = new ArtificialChromosome[Config.NumberOfChildrenLambda];
                var braninRcos = new BraninRcos();
              
                var oneFifthTracker = 0;
                for (int i = 0; i < children.Length; i++)
                {
                    var child = new ArtificialChromosome();
                    var randomParentIndex = new Random().Next(parentPopulation.Length); // between 0 and length - 1
                    var randomParent = parentPopulation[randomParentIndex];

                    child.X1 = randomParent.X1 + (randomParent.MutationSigmaX1 * GetARandomNormarDistributionNumber(0, 1));
                    child.X2 = randomParent.X2 + (randomParent.MutationSigmaX2 * GetARandomNormarDistributionNumber(0, 1));
                    child.ObjectiveFunctionResult = braninRcos.BraninRcosObjectiveFunction(child.X1, child.X2);

                    child.MutationSigmaX1 = randomParent.MutationSigmaX1;
                    child.MutationSigmaX2 = randomParent.MutationSigmaX2;
                    if (randomParent.ObjectiveFunctionResult > child.ObjectiveFunctionResult)
                    {
                        oneFifthTracker++;
                    }
                    
                    child.MutationSigmaX1 = Config.InitialSigmaX1;
                    child.MutationSigmaX2 = Config.InitialSigmaX2;
                    children[i] = child;
                }

                // adjust sigma
                if (oneFifthTracker > Config.NumberOfChildrenLambda / 5)
                {
                    // increase sigma
                    Config.InitialSigmaX1 = Config.InitialSigmaX1 +
                                            Config.OneOverFiveSigmaRuleConstant * Config.InitialSigmaX1;
                    Config.InitialSigmaX2 = Config.InitialSigmaX2 +
                                            Config.OneOverFiveSigmaRuleConstant * Config.InitialSigmaX2;
                }
                else if (oneFifthTracker < Config.NumberOfChildrenLambda / 5)
                {
                    // increase sigma
                    Config.InitialSigmaX1 = Config.InitialSigmaX1 -
                                            Config.OneOverFiveSigmaRuleConstant * Config.InitialSigmaX1;
                    Config.InitialSigmaX2 = Config.InitialSigmaX2 -
                                            Config.OneOverFiveSigmaRuleConstant * Config.InitialSigmaX2;
                }


                ArtificialChromosome[] parents = TakeBestForNextGenParents(children);
                return parents;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static ArtificialChromosome[] TakeBestForNextGenParents(ArtificialChromosome[] children)
        {
            try
            {
                var childrenOrdered = children.OrderBy(x => x.ObjectiveFunctionResult);
                var nextGenParents = childrenOrdered.Take(Config.NumberOfParentsMiu).ToArray();
                return nextGenParents;
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

        private static ArtificialChromosome GetBestSolution(ArtificialChromosome[] population)
        {
            ArtificialChromosome bestSolution = new ArtificialChromosome { ObjectiveFunctionResult = Double.MaxValue};
            for (var i = 0; i < Config.NumberOfParentsMiu; i++)
            {
                bestSolution = population[i].ObjectiveFunctionResult < bestSolution.ObjectiveFunctionResult ? population[i] : bestSolution;
            }
            return bestSolution;
        }


        public static ArtificialChromosome[] GenerateInitialPopulation()
        {
            ArtificialChromosome[] initialPopulation = new ArtificialChromosome[Config.NumberOfParentsMiu];
            var randomGenerator = new RandomDouble();
            var braninRcos = new BraninRcos();
            for (var i = 0; i < Config.NumberOfParentsMiu; i++)
            {
                var solution = new ArtificialChromosome();

                do
                {
                    solution.X1 = randomGenerator.GetRandomNumber(Config.MinX1, Config.MaxX1);
                    solution.X2 = randomGenerator.GetRandomNumber(Config.MinX2, Config.MaxX2);
                } while (i != 0 &&
                         Math.Abs(solution.X1 - initialPopulation[i - 1].X1) < 0.000000000000001 &&
                         Math.Abs(solution.X2 - initialPopulation[i - 1].X2) < 0.000000000000001);

                solution.MutationSigmaX1 = Config.InitialSigmaX1;
                solution.MutationSigmaX2 = Config.InitialSigmaX2;
                solution.ObjectiveFunctionResult = braninRcos.BraninRcosObjectiveFunction(solution.X1, solution.X2);
                initialPopulation[i] = solution;
            }
            return initialPopulation;
        }

        public static void DisplayChromosome(double x1, double x1Sigma, double x2, double x2Sigma, double result)
        {
            Console.WriteLine("{0} \t {1} \t {2} \t {3} \t {4} ", x1, x1Sigma, x2, x2Sigma, result);
        }

        public static void DisplayChromosome(ArtificialChromosome solution)
        {
            DisplayChromosome(solution.X1, solution.MutationSigmaX1, solution.X2, solution.MutationSigmaX2, solution.ObjectiveFunctionResult);
        }
    }

    public static class Config
    {
        public static int NumberOfParentsMiu = 30;
        public static int NumberOfChildrenLambda = NumberOfParentsMiu * 6;
        public static int NumberOfGenerationsM = 60;
        public static double OneOverFiveSigmaRuleConstant = 0.85;
        public static double InitialX1 = 0;
        public static double InitialX2 = 8;
        public static double InitialSigmaX1 = 1.25;
        public static double InitialSigmaX2 = 1;
        public static double MinX1 = -5;
        public static double MaxX1 = 10;
        public static double MinX2 = 0;
        public static double MaxX2 = 15;
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

    public class RandomDouble
    {
        // https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers
        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }

    public class ArtificialChromosome
    {
        public double X1 { get; set; }
        public double MutationSigmaX1 { get; set; }
        public double X2 { get; set; }
        public double MutationSigmaX2 { get; set; }
        public double ObjectiveFunctionResult { get; set; }

    }

    public class GenerationDetails
    {
        public int Number { get; set; }
        public ArtificialChromosome BastOfGeneration { get; set; }

    }
}
