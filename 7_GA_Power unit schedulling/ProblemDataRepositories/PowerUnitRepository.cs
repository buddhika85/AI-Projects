using System.Collections.Generic;
using System.Linq;
using _7_GA_Power_unit_schedulling.Model;

namespace _7_GA_Power_unit_schedulling.ProblemDataRepositories
{
    public class PowerUnitRepository
    {
        public List<PowerUnit> PowerUnits { get; set; }
        public double MaxCapacity { get; set; }

        public PowerUnitRepository()
        {
            PowerUnits = new List<PowerUnit>
            {
                new PowerUnit
                {
                    UnitNumber = 1,
                    UnitCapacity = 20,
                    NumberOfIntervalsRequiredForMaintainance = 2
                },
                new PowerUnit
                {
                    UnitNumber = 2,
                    UnitCapacity = 15,
                    NumberOfIntervalsRequiredForMaintainance = 2
                },
                new PowerUnit
                {
                    UnitNumber = 3,
                    UnitCapacity = 35,
                    NumberOfIntervalsRequiredForMaintainance = 1
                },
                new PowerUnit
                {
                    UnitNumber = 4,
                    UnitCapacity = 40,
                    NumberOfIntervalsRequiredForMaintainance = 1
                },
                new PowerUnit
                {
                    UnitNumber = 5,
                    UnitCapacity = 15,
                    NumberOfIntervalsRequiredForMaintainance = 1
                },
                new PowerUnit
                {
                    UnitNumber = 6,
                    UnitCapacity = 15,
                    NumberOfIntervalsRequiredForMaintainance = 1
                },
                new PowerUnit
                {
                    UnitNumber = 7,
                    UnitCapacity = 10,
                    NumberOfIntervalsRequiredForMaintainance = 1
                },
            };

            MaxCapacity = PowerUnits.Sum(x => x.UnitCapacity);
        }

        public List<PowerUnit> GetAllPowerUnits()
        {
            try
            {
                return PowerUnits;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
