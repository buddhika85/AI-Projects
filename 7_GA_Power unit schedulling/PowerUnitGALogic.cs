using System;
using Encog.ML.EA.Genome;
using Encog.ML.EA.Population;
using Encog.ML.EA.Species;
using Encog.ML.Genetic.Genome;

namespace _7_GA_Power_unit_schedulling
{
    public class PowerUnitGALogic
    {
        public BasicPopulation CreateInitialPopulation(int populationSize, int geneCountPerChomosome)
        {
            try
            {
                //var genomeFactory = new BasicGenome(geneCountPerChomosome);
                ////var population = new BasicPopulation(populationSize, genomeFactory);
                //var defaultSpecies = new BasicSpecies();
                //for (var i = 1; i <= populationSize; i++)
                //{
                //    Console.Write($"\n {i} Chomosome - ");
                //    //IntegerArrayGenome genome = CreateRandomGenome(geneCountPerChromosome);
                //    defaultSpecies.Members.Add(genome);
                //}
                //population.Species.Add(defaultSpecies);
                //return population;
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}