using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Models
{
    public class BoundSubUnit: SubUnit
    {
        protected BoundSubUnit() { }

        public BoundSubUnit(BoundSubUnitParameters argument, Entrance entrance, AdministrationUnit administrationUnit)
        {
            Floor = argument.Floor;
            Id = argument.Id;
            Title = argument.Title;
            Number = argument.Number;
            Usage = argument.Usage;
            Version = argument.Version;
            Entrance = entrance;
            AdministrationUnit = administrationUnit;
        }

        public virtual string Floor
        {
            get;
            protected set;
        }

        public virtual Entrance Entrance
        {
            get;
            protected set;
        }
    }
}
