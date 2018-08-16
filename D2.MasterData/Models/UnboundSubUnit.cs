using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Models
{
    public class UnboundSubUnit: SubUnit
    {
        protected UnboundSubUnit() { }
        public UnboundSubUnit(UnboundSubUnitParameters argument)
        {
            Id = argument.Id;
            Title = argument.Title;
            Number = argument.Number;
            Usage = argument.Usage;
            Version = argument.Version;
        }
    }
}
