using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Parameters
{
    public class UnboundSubUnitParameters: SubUnitParameters
    {
        public UnboundSubUnitType? Type { get; set; }
    }

    public enum UnboundSubUnitType { Parkinglot, Antenna   }
}
