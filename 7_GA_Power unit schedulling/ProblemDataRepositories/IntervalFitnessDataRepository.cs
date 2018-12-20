using _7_GA_Power_unit_schedulling.Model;
using System.Collections.Generic;

namespace _7_GA_Power_unit_schedulling.ProblemDataRepositories
{
    public class IntervalFitnessDataRepository
    {
        public List<IntervalsFitnessData> IntervalRawData { get; set; }
       

        public IntervalFitnessDataRepository(double maxReserve)
        {
            IntervalRawData = new List<IntervalsFitnessData>
            {
                new IntervalsFitnessData
                {
                    IntervalId = 1,
                    MaxReserve = maxReserve,
                    PowerRequirement = 80,
                    ReducedAmountOnMaintanace = 0,
                    ReserveAfterMaintainance = maxReserve - 0 - 80
                },
                new IntervalsFitnessData
                {
                    IntervalId = 2,
                    MaxReserve = maxReserve,
                    PowerRequirement = 90,
                    ReducedAmountOnMaintanace = 0,
                    ReserveAfterMaintainance = maxReserve - 0 - 90
                },
                new IntervalsFitnessData
                {
                    IntervalId = 3,
                    MaxReserve = maxReserve,
                    PowerRequirement = 65,
                    ReducedAmountOnMaintanace = 0,
                    ReserveAfterMaintainance = maxReserve - 0 - 65
                },
                new IntervalsFitnessData
                {
                    IntervalId = 4,
                    MaxReserve = maxReserve,
                    PowerRequirement = 70,
                    ReducedAmountOnMaintanace = 0,
                    ReserveAfterMaintainance = maxReserve - 0 - 70
                },
            };
         
        }

        public List<IntervalsFitnessData> GetAllIntervalsRawData()
        {
            try
            {
                return IntervalRawData;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
