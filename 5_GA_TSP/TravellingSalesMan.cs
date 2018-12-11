using Encog.MathUtil;
using Encog.ML.EA.Population;
using Encog.ML.EA.Species;
using Encog.ML.EA.Train;
using Encog.ML.Genetic.Crossover;
using Encog.ML.Genetic.Genome;
using Encog.ML.Genetic.Mutate;
using System;

namespace _5_GA_TSP
{
    public class TravellingSalesMan
    {
        public BasicPopulation CreateInitialPopulation(int populationSize, int geneCountPerChromosome)
        {
            try
            {
                var genomeFactory = new IntegerArrayGenomeFactory(geneCountPerChromosome);
                var population = new BasicPopulation(populationSize, genomeFactory);
                var defaultSpecies = new BasicSpecies();
                for (var i = 1; i <= populationSize; i++)
                {
                    Console.Write($"\n {i} Chomosome - ");
                    IntegerArrayGenome genome = CreateRandomGenome(geneCountPerChromosome);
                    defaultSpecies.Members.Add(genome);
                }
                population.Species.Add(defaultSpecies);
                return population;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IntegerArrayGenome CreateRandomGenome(int chomosomeLength)
        {
            try
            {
                var cityNumbers = new int[chomosomeLength];
                for (var i = 0; i < cityNumbers.Length; i++)
                {
                    cityNumbers[i] = i;     // [0,1,2,3,4,5,6,7,8,9,0,11,12,13,14,15,16,17,18,19]
                }

                Shuffle(cityNumbers);       // shuffle
                var integerGenome = new IntegerArrayGenome(chomosomeLength);
                for (var i = 0; i < cityNumbers.Length; i++)
                {
                    integerGenome.Data[i] = cityNumbers[i];
                    if (i == 0)
                    {
                        Console.Write($"[{cityNumbers[i]}");
                    }
                    else if (i < cityNumbers.Length - 1)
                    {
                        Console.Write($", {cityNumbers[i]}");
                    }
                    else
                    {
                        Console.Write($", {cityNumbers[i]}]");
                    }
                }

                return integerGenome;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// https://www.dotnetperls.com/fisher-yates-shuffle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public void Shuffle<T>(T[] array)
        {
            Random _random = new Random();
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                int r = i + _random.Next(n - i);
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }



        /// <summary>
        /// create cities 
        /// </summary>
        /// <param name="cityCount"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        /// <returns></returns>
        public City[] LoadCities(int cityCount, int maxX, int maxY)
        {
            try
            {
                var cities = new City[cityCount];
                for (var i = 0; i < cityCount; i++)
                {
                    var x = (int)(ThreadSafeRandom.NextDouble() * maxX);
                    var y = (int)(ThreadSafeRandom.NextDouble() * maxY);
                    var city = new City(x, y);
                    cities[i] = city;
                    Console.WriteLine($"{i + 1} City - X = {x} , Y = {y} created");
                }

                return cities;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public TrainEA CreateGA(int cityCount, BasicPopulation population, TravellingSalesManFitnessScore tspFitness, double crossOverProbabality, double mutationProbabality)
        {
            try
            {
                var geneticAlgorithm = new TrainEA(population, tspFitness);
                geneticAlgorithm.AddOperation(crossOverProbabality, new SpliceNoRepeat(cityCount / 3));
                geneticAlgorithm.AddOperation(mutationProbabality, new MutateShuffle());
                return geneticAlgorithm;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public TrainEA RunGA(TrainEA geneticAlgorithm, int maxSameSolutionCount)
        {
            try
            {
                var sameSolutionCount = 0;
                var lastSolutionError = double.MaxValue;
                var iteration = 1;
                while (sameSolutionCount < maxSameSolutionCount)
                {
                    geneticAlgorithm.Iteration();
                    var currentSolutionError = geneticAlgorithm.Error;
                    if (Math.Abs(lastSolutionError - currentSolutionError) < 1.0)
                    {
                        sameSolutionCount++;
                    }
                    else
                    {
                        sameSolutionCount = 0;
                    }

                    lastSolutionError = currentSolutionError;
                    Console.WriteLine($"{iteration++} Iteration - Error {currentSolutionError}");
                }
                geneticAlgorithm.FinishTraining();
                Console.WriteLine("Good Solutions Found");
                return geneticAlgorithm;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void DisplaySolution(TrainEA geneticAlgorithm, City [] cities)
        {
            try
            {
                var bestChromosome = (IntegerArrayGenome) geneticAlgorithm.Population.BestGenome;
                foreach (var gene in bestChromosome.Data)
                {
                    Console.WriteLine($"city - {gene} => X = {cities[gene].X}, Y = {cities[gene].Y}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
