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

        public UnboundSubUnit(UnboundSubUnitParameters argument, AdministrationUnit administrationUnit)
        {
            Id = argument.Id;
            Title = argument.Title;
            Number = argument.Number;
            Version = argument.Version;
            AdministrationUnit = administrationUnit;
            Type = argument.Type;
        }


        public virtual UnboundSubUnitType? Type
        {
            get;
            protected set;
        }
    }
}
