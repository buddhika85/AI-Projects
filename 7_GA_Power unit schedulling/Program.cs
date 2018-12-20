using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog.ML.EA.Population;
using _7_GA_Power_unit_schedulling.Model;
using _7_GA_Power_unit_schedulling.ProblemDataRepositories;

namespace _7_GA_Power_unit_schedulling
{
    class Program
    {
        static void Main(string[] args)
        {
            var powerUnitReposioty = new PowerUnitRepository();
            var powerUnitGALogic = new PowerUnitGALogic();
            var populationSize = 50;
            var crossOverProbabality = 0.9;
            var mutationProbabaity = 0.1;


            Display("------------ Load Power Unit Details");
            var powerUnits = powerUnitReposioty.GetAllPowerUnits();
            DisplayPowerUnitData(powerUnits);

            Display("------------ Create Initial Population");
            var population = powerUnitGALogic.CreateInitialPopulation(populationSize, powerUnits.Count);

            // 2 create fitness function
            Display("\n------------ Create Fitness function - sum of total diatance of cities within the chromosome - distance low --> better chromosome");
            var tspFitness = new TravellingSalesManFitnessScore(cities);

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
