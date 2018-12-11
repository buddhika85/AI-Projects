using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog.ML;
using Encog.ML.Genetic.Genome;
using Encog.Neural.Networks.Training;

namespace _5_GA_TSP
{
    // Ref - https://github.com/encog/encog-dotnet-core/blob/master/ConsoleExamples/Examples/GeneticTSP/TSPScore.cs
    public class TravellingSalesManFitnessScore : ICalculateScore
    {
        private readonly City[] cities;

        public TravellingSalesManFitnessScore(City[] cities)
        {
            this.cities = cities;
        }

        #region ICalculateGenomeScore Members

        /// <summary>
        /// Returns total sum of distance between cities represnted in the chromosome
        /// </summary>
        /// <param name="phenotype"></param>
        /// <returns></returns>
        public double CalculateScore(IMLMethod phenotype)
        {
            double result = 0.0;
            IntegerArrayGenome genome = (IntegerArrayGenome)phenotype;
            int[] path = ((IntegerArrayGenome)genome).Data;

            for (int i = 0; i < cities.Length - 1; i++)
            {
                City city1 = cities[path[i]];
                City city2 = cities[path[i + 1]];

                double dist = city1.Proximity(city2);
                result += dist;
            }

            return result;
        }

        public bool ShouldMinimize
        {
            get { return true; }
        }

        #endregion

        /// <inheritdoc/>
        public bool RequireSingleThreaded
        {
            get { return false; }
        }
    }
}
