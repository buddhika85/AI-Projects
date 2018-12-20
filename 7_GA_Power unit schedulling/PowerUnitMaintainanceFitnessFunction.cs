using System.Collections.Generic;
using System.Linq;
using _7_GA_Power_unit_schedulling.EncogExtensions;
using Encog.ML;
using Encog.Neural.Networks.Training;
using _7_GA_Power_unit_schedulling.Model;

namespace _7_GA_Power_unit_schedulling
{
    public class PowerUnitMaintainanceFitnessFunction : ICalculateScore
    {
        public PowerUnit[] PowerUnits { get; set; }
        public List<IntervalsFitnessData> IntervalData { get; set; }

        public PowerUnitMaintainanceFitnessFunction(PowerUnit[] powerUnits, List<IntervalsFitnessData> intervalRawData)
        {
            PowerUnits = powerUnits;
            IntervalData = intervalRawData;
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
            FourBitCustomGenome genome = (FourBitCustomGenome)phenotype;
            FourBitGene [] genomeData = ((FourBitCustomGenome)genome).Data;

            //for (int i = 0; i < IntervalData.Count; i++)
            //{
            //    IntervalsFitnessData interval = IntervalData[i];
            //    for (int j = 0; i < genomeData.Length; j++)
            //    {
            //        PowerUnit powerUnit = PowerUnits[j];
            //        FourBitGene fourBitGene = genomeData[j];
            //        int geneBit = fourBitGene.Gene[interval.IntervalId - 1];
            //    }
            //}

           

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
