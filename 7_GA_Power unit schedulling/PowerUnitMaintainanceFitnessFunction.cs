using System;
using System.Collections.Generic;
using System.Linq;
using _7_GA_Power_unit_schedulling.EncogExtensions;
using Encog.ML;
using Encog.ML.Prg.Train;
using Encog.Neural.Networks.Training;
using _7_GA_Power_unit_schedulling.Model;
using _7_GA_Power_unit_schedulling.ProblemDataRepositories;

namespace _7_GA_Power_unit_schedulling
{
    public class PowerUnitMaintainanceFitnessFunction : ICalculateScore
    {
        public PowerUnit[] PowerUnits { get; set; }
        public int NumberOfIntervals { get; set; }
        public double MaxPossiblePower { get; set; }
        

        public PowerUnitMaintainanceFitnessFunction(PowerUnit[] powerUnits, int numberOfIntervals, double maxPossiblePower)
        {
            PowerUnits = powerUnits;
            NumberOfIntervals = numberOfIntervals;
            MaxPossiblePower = maxPossiblePower;
        }

        #region ICalculateGenomeScore Members

        /// <summary>
        /// Returns total sum of distance between cities represnted in the chromosome
        /// </summary>
        /// <param name="phenotype"></param>
        /// <returns></returns>
        public double CalculateScore(IMLMethod phenotype)
        {
            try
            {
                FourBitCustomGenome genome = (FourBitCustomGenome)phenotype;
                FourBitGene [] genomeData = ((FourBitCustomGenome)genome).Data;
                //double maxPossiblePower = PowerUnits.Sum(x => x.UnitCapacity);

                new PowerUnitGALogic().DisplayGeneAsString(genome, genomeData);

                var intervalFitnessDataRepository = new IntervalFitnessDataRepository(MaxPossiblePower);
                var intervalRawData = intervalFitnessDataRepository.IntervalRawData;

                for (int i = 0; i < NumberOfIntervals; i++)
                {
                    IntervalsFitnessData interval = intervalRawData[i];
                    //interval.MaxReserve = maxPossiblePower;
                    for (int j = 0; j < genomeData.Length; j++)
                    {
                        PowerUnit powerUnit = PowerUnits[j];
                        FourBitGene fourBitGene = genomeData[j];
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
                    //if (interval.ReserveAfterMaintainance < 0.0)
                    //{
                    //    // the chromosome is not suitable for out requirement
                    //    chromosomeFitness = 0.0;
                    //}
                }

                var reserveAfterMaintainanceMin = intervalRawData.Min(x => x.ReserveAfterMaintainance);
                // minimal rerserve after maintainance and usage provides chormosomes fitness
                var chromosomeFitness = reserveAfterMaintainanceMin > 0.0 ? reserveAfterMaintainanceMin : 0.0;
                Console.WriteLine("\tFitness = " + chromosomeFitness);
                return chromosomeFitness;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool ShouldMinimize
        {
            get { return false; }
        }

        #endregion

        /// <inheritdoc/>
        public bool RequireSingleThreaded
        {
            get { return true; }
        }
    }
}
