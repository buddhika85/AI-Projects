using _7_GA_Power_unit_schedulling.EncogExtensions;
using Encog.ML.EA.Population;
using Encog.ML.EA.Species;
using System;
using System.Collections.Generic;
using System.Linq;
using Encog.ML.EA.Train;
using Encog.ML.Genetic.Crossover;
using Encog.ML.Genetic.Mutate;
using _7_GA_Power_unit_schedulling.Model;
using _7_GA_Power_unit_schedulling.ProblemDataRepositories;

namespace _7_GA_Power_unit_schedulling
{
    public class PowerUnitGALogic
    {
        public BasicPopulation CreateInitialPopulation(int populationSize, int geneCountPerChomosome)
        {
            try
            {
                var genomeFactory = new FourBitChomosomeGenomeFactory(geneCountPerChomosome);
                var population = new BasicPopulation(populationSize, genomeFactory);
                var defaultSpecies = new BasicSpecies();
                for (var i = 1; i <= populationSize; i++)
                {
                    Console.Write("\n {0} Chromosome - ", i);    
                    FourBitCustomGenome genome = CreateRandomGenome(geneCountPerChomosome);
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

        public TrainEA CreateGA(int powerUnitCount, BasicPopulation population, PowerUnitMaintainanceFitnessFunction maintainanceFitness, double crossOverProbabality, double mutationProbabality)
        {
            try
            {
                var geneticAlgorithm = new TrainEA(population, maintainanceFitness);
                geneticAlgorithm.AddOperation(crossOverProbabality, new CustomCrossOver());
                geneticAlgorithm.AddOperation(mutationProbabality, new CustomMutation());
                return geneticAlgorithm;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private FourBitCustomGenome CreateRandomGenome(int geneCountPerChromosome)
        {
            try
            {
                var randomGenome = new FourBitCustomGenome(geneCountPerChromosome);
                var powerUnits = new PowerUnitRepository().GetAllPowerUnits();
                var chromosomeData = new FourBitGene[geneCountPerChromosome];
                for (var powerUnitIndex = 0; powerUnitIndex < chromosomeData.Length; powerUnitIndex++)
                {
                    var powerUnitInfo = powerUnits.SingleOrDefault(x => x.UnitNumber == powerUnitIndex + 1);
                    chromosomeData[powerUnitIndex] = GetRandomGeneForPowerUnit(powerUnitInfo);
                }

                DisplayGeneAsString(randomGenome, chromosomeData);

                return randomGenome;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void DisplayGeneAsString(FourBitCustomGenome randomGenome, FourBitGene[] chromosomeData)
        {
            for (var i = 0; i < randomGenome.Data.Length; i++)
            {
                randomGenome.Data[i] = chromosomeData[i];

                // display chromosome to user
                var geneBitsString = GetGeneBitString(randomGenome.Data[i]);
                if (i == 0)
                {
                    Console.Write("[ {0}", geneBitsString);
                }
                else if (i < chromosomeData.Length - 1)
                {
                    Console.Write(", {0}", geneBitsString);
                }
                else
                {
                    Console.Write(", {0}]", geneBitsString);
                }
            }
        }

        /// <summary>
        /// Returns gene as readable string like [0,1,1,0]
        /// </summary>
        /// <param name="fourBitGene"></param>
        /// <returns></returns>
        private string GetGeneBitString(FourBitGene fourBitGene)
        {
            try
            {
                var gene = fourBitGene.Gene;
                var geneString = "[";
                for (var i = 0; i < gene.Length; i++)
                {
                    if (i != gene.Length - 1) // 1 != 3
                    {
                        geneString += gene[i] + ",";
                    }
                    else
                    {
                        geneString += gene[i];
                    }
                }
                geneString += "]";
                return geneString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private FourBitGene GetRandomGeneForPowerUnit(PowerUnit powerUnitInfo)
        {
            try
            {
                var randomGene = new FourBitGene();
                var random = new Random();
                var randomNumber = random.Next(1, 101); // random number is between 0 and 100

                // add maintainace gene bits
                switch (powerUnitInfo.NumberOfIntervalsRequiredForMaintainance)
                {
                    case 1:
                    {
                        var remainder = randomNumber % 4;        // either zero or one or two or three
                        randomGene.Gene = remainder == 0 ? new[] {0, 0, 0, 1} :
                            remainder == 1 ? new[] {0, 0, 1, 0} :
                            remainder == 2 ? new[] {0, 1, 0, 0} : new[] {1, 0, 0, 0};
                        break;
                    }
                    case 2:
                    {
                        var remainder = randomNumber % 3;        // either zero or one or two
                        randomGene.Gene = remainder == 0 ? new[] {0, 0, 1, 1} :
                            remainder == 1 ? new[] {0, 1, 1, 0} : new[] { 1, 1, 0, 0 };
                        break;
                    }
                    case 3:
                    {
                        var remainder = randomNumber % 2;        // either zero or one
                        randomGene.Gene = remainder == 0 ? new[] { 0, 1, 1, 1 } : new[] { 1, 1, 1, 0 };
                        break;
                    }
                    case 4:
                    {
                        var remainder = randomNumber % 2;        // either zero or one
                        randomGene.Gene = remainder == 0 ? new[] {0, 0, 0, 0} : new[] { 1, 1, 1, 1 };
                        break;
                    }
                    default:
                    {
                        randomGene.Gene = new[] {0, 0, 0, 0};
                        break;
                    }
                }
                return randomGene;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public TrainEA RunGA(TrainEA geneticAlgorithm, int maxNumIterationsSameSolution)
        {
            try
            {
                var sameSolutionCount = 0;
                var lastSolutionError = double.MaxValue;
                var iteration = 1;
                while (sameSolutionCount < maxNumIterationsSameSolution)
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

        public void DisplaySolution(TrainEA geneticAlgorithm, PowerUnit[] powerUnits, double maxPossiblePower)
        {
            try
            {
                var bestChromosome = (FourBitCustomGenome)geneticAlgorithm.Population.BestGenome;
                FourBitGene [] bestChomosomeGenes = bestChromosome.Data;

                // display best chromosome genes
                for (var i = 0; i < bestChomosomeGenes.Length; i++)
                {
                    // display chromosome to user
                    var geneBitsString = GetGeneBitString(bestChomosomeGenes[i]);
                    if (i == 0)
                    {
                        Console.Write("Best Chromosome Genes = [ {0}", geneBitsString);
                    }
                    else if (i < bestChomosomeGenes.Length - 1)
                    {
                        Console.Write(", {0}", geneBitsString);
                    }
                    else
                    {
                        Console.Write(", {0}]\n\n", geneBitsString);
                    }
                }

                var intervalFitnessDataRepository = new IntervalFitnessDataRepository(maxPossiblePower);
                var intervalRawData = intervalFitnessDataRepository.IntervalRawData;

                for (int i = 0; i < intervalRawData.Count; i++)
                {
                    IntervalsFitnessData interval = intervalRawData[i];
                    for (int j = 0; j < bestChomosomeGenes.Length; j++)
                    {
                        PowerUnit powerUnit = powerUnits[j];
                        FourBitGene fourBitGene = bestChomosomeGenes[j];
                        int geneBitIndex = i;
                        var isPowerUnitMaintained = fourBitGene.Gene[geneBitIndex] == 1;
                        if (isPowerUnitMaintained)
                        {
                            interval.ReducedAmountOnMaintainance = interval.ReducedAmountOnMaintainance + (1 * powerUnit.UnitCapacity);
                        }
                        else
                        {
                            interval.ReducedAmountOnMaintainance = interval.ReducedAmountOnMaintainance + (0 * powerUnit.UnitCapacity);
                        }
                    }

                    var totalPowerReductionOnMaintanceAndUsage =
                        interval.PowerRequirement + interval.ReducedAmountOnMaintainance;
                    interval.ReserveAfterMaintainance = interval.MaxReserve - totalPowerReductionOnMaintanceAndUsage;
                    if (interval.ReserveAfterMaintainance < 0.0)
                    {
                        // the chromosome is not suitable for out requirement
                        Console.WriteLine("Error - On Interval {0} has net reserve of {1} ", interval.IntervalId, interval.ReserveAfterMaintainance);
                    }
                }

                foreach (var interval in intervalRawData)
                {
                    Console.WriteLine(
                        "Interval Id = {0} , Max Reserve = {1}, Power Requirement = {2} , Reduced on maintainance = {3} , Reserve after Maintainance = {4}",
                        interval.IntervalId, interval.MaxReserve, interval.PowerRequirement,
                        interval.ReducedAmountOnMaintainance, interval.ReserveAfterMaintainance);
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