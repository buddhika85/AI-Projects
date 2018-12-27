using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog.ML.EA.Population;
using Encog.ML.EA.Train;
using _7_GA_Power_unit_schedulling.Model;
using _7_GA_Power_unit_schedulling.ProblemDataRepositories;

namespace _7_GA_Power_unit_schedulling
{
    class Program
    {
        static void Main(string[] args)
        {
            var powerUnitRepository = new PowerUnitRepository();
           
            var powerUnitGALogic = new PowerUnitGALogic();
            var populationSize = 500;
            var crossOverProbabality = 0.9;
            var mutationProbabaity = 0.001;

            const int MaxNumIterationsSameSolution = 25;     // maximum number of iterations which GA could run on a same error rate


            Display("------------ Load Power Unit Details");
            var powerUnits = powerUnitRepository.PowerUnits;
            DisplayPowerUnitData(powerUnits);

            // 1 create initial population
            Display("------------ Create Initial Population");
            var population = powerUnitGALogic.CreateInitialPopulation(populationSize, powerUnits.Count);

            // 2 create fitness function
            Display("\n------------ Create Fitness function - sum of total diatance of cities within the chromosome - distance low --> better chromosome");
            double maxPossiblePower = powerUnits.Sum(x => x.UnitCapacity);
            var numberOfIntervals = new IntervalFitnessDataRepository(maxPossiblePower).GetNumberOfIntervals();
            var powerUnitMaintainanceFitness = new PowerUnitMaintainanceFitnessFunction(powerUnitRepository.GetAllPowerUnits(), numberOfIntervals, maxPossiblePower);

            // 3 create GA trainer
            Display("\n------------ Create GA Trainer for iterations");
            TrainEA geneticAlgorithm = powerUnitGALogic.CreateGA(powerUnits.Count, population, powerUnitMaintainanceFitness, crossOverProbabality, mutationProbabaity);

            // 4 iterate and create off spring of new solutions
            Display("\n------------ Run GA for iterations until good solutions found");
            geneticAlgorithm = powerUnitGALogic.RunGA(geneticAlgorithm, MaxNumIterationsSameSolution);

            // 5 display GA results
            Display("\n------------ Display Final solution after iterations");
            powerUnitGALogic.DisplaySolution(geneticAlgorithm, powerUnitRepository.GetAllPowerUnits(), maxPossiblePower);

            Display("\n------------ Done");
            Console.ReadKey();
        }

        private static void DisplayPowerUnitData(List<PowerUnit> powerUnits)
        {
            try
            {
                Console.WriteLine("Unit \t Capacity \t Maintaiance Intervals");
                foreach (var powerUnit in powerUnits)
                {
                    Console.WriteLine("{0} \t {1} \t\t {2}", powerUnit.UnitNumber, powerUnit.UnitCapacity, powerUnit.NumberOfIntervalsRequiredForMaintainance);
                }
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
