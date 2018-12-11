using Encog.MathUtil;
using System;
using System.Collections.Generic;
using Encog.ML.EA.Population;
using Encog.ML.EA.Species;
using Encog.ML.EA.Train;
using Encog.ML.Genetic.Genome;

namespace _5_GA_TSP
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Display("------------ Load City Coodinates");
                var travellingSalesman = new TravellingSalesMan();
                var cities = travellingSalesman.LoadCities(Config.CityCount, Config.MaxXCordinate, Config.MaxYCordinate);

                // 1 create initial population
                Display("\n------------ Create Initial Solution Population");
                BasicPopulation population = travellingSalesman.CreateInitialPopulation(Config.PopulationSize, Config.CityCount);

                // 2 create fitness function
                Display("\n------------ Create Fitness function - sum of total diatance of cities within the chromosome - distance low --> better chormosome");
                var tspFitness = new TravellingSalesManFitnessScore(cities);
                
                // 3 create GA trainer
                Display("\n------------ Create GA Trainer for iterations");
                TrainEA geneticAlgorithm = travellingSalesman.CreateGA(Config.CityCount, population, tspFitness, Config.CrossOverProbabality, Config.MutationProbabality);

                // 4 iterate and create off spring of new solutions
                Display("\n------------ Run GA for iterations until good solutions found");
                geneticAlgorithm = travellingSalesman.RunGA(geneticAlgorithm, Config.MaxNumIterationsSameSolution);

                // 5 display GA results
                Display("\n------------ Display Final solution after iterations");
                travellingSalesman.DisplaySolution(geneticAlgorithm, cities);

                Display("\n------------ Fin");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void Display(string message)
        {
            Console.WriteLine(message);
        }
    }
}
