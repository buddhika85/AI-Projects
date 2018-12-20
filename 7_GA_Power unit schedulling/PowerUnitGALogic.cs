using _7_GA_Power_unit_schedulling.EncogExtensions;
using Encog.ML.EA.Population;
using Encog.ML.EA.Species;
using System;
using System.Linq;
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

        private FourBitCustomGenome CreateRandomGenome(int geneCountPerChromosome)
        {
            try
            {
                var randomGenome = new FourBitCustomGenome(geneCountPerChromosome);
                var powerUnits = new PowerUnitRepository().GetAllPowerUnits();
                var chromosomeData = new FourBitGene[geneCountPerChromosome];
                for (var powerUnitNumber = 1; powerUnitNumber < chromosomeData.Length; powerUnitNumber++)
                {
                    var powerUnitInfo = powerUnits.SingleOrDefault(x => x.UnitNumber == powerUnitNumber);
                    chromosomeData[powerUnitNumber] = GetRandomGeneForPowerUnit(powerUnitInfo);
                }

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

                return randomGenome;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
                        geneString += gene + ",";
                    }
                    else
                    {
                        geneString += gene;
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

      
    }
}