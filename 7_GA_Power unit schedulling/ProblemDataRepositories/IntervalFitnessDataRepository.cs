using System;
using _7_GA_Power_unit_schedulling.Model;
using System.Collections.Generic;

namespace _7_GA_Power_unit_schedulling.ProblemDataRepositories
{
    public class IntervalFitnessDataRepository
    {
        public List<IntervalsFitnessData> IntervalRawData { get; set; }

        public int GetNumberOfIntervals()
        {
            try
            {
                return IntervalRawData.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IntervalFitnessDataRepository(double maxReserve)
        {
            IntervalRawData = new List<IntervalsFitnessData>
            {
                new IntervalsFitnessData
                {
                    IntervalId = 1,
                    MaxReserve = maxReserve,
                    PowerRequirement = 80,
                    ReducedAmountOnMaintainance = 0,
                    ReserveAfterMaintainance = maxReserve - 0 - 80
                },
                new IntervalsFitnessData
                {
                    IntervalId = 2,
                    MaxReserve = maxReserve,
                    PowerRequirement = 90,
                    ReducedAmountOnMaintainance = 0,
                    ReserveAfterMaintainance = maxReserve - 0 - 90
                },
                new IntervalsFitnessData
                {
                    IntervalId = 3,
                    MaxReserve = maxReserve,
                    PowerRequirement = 65,
                    ReducedAmountOnMaintainance = 0,
                    ReserveAfterMaintainance = maxReserve - 0 - 65
                },
                new IntervalsFitnessData
                {
                    IntervalId = 4,
                    MaxReserve = maxReserve,
                    PowerRequirement = 70,
                    ReducedAmountOnMaintainance = 0,
                    ReserveAfterMaintainance = maxReserve - 0 - 70
                },
            };
         
        }
    }
}
