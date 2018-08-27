using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace D2.MasterData.Models
{
    public class Entrance : BaseModel
    {
        protected Entrance()
        {
            _subUnits = new List<BoundSubUnit>();
        }

        public Entrance(EntranceParameters argument, AdministrationUnit unit)
        {
            Id = argument.Id;
            Title = argument.Title;
            Address = new Address(argument.Address);
            Version = argument.Version;
            AdministrationUnit = unit;
            if (argument.SubUnits != null)
            {
                var parameter = from subUnitParameters in argument.SubUnits
                                 select new BoundSubUnit(subUnitParameters, this, unit);
                _subUnits = new List<BoundSubUnit>(parameter);
            }
        }

        public virtual string Title
        {
            get;
            protected set;
        }

        public virtual Address Address
        {
            get;
            protected set;
        }

        public virtual AdministrationUnit AdministrationUnit
        {
            get;
            protected set;
        }


        public virtual int Version
        {
            get;
            protected set;
        }

        private IList<BoundSubUnit> _subUnits;

        public virtual IEnumerable<BoundSubUnit> SubUnits
        {
            get { return _subUnits; }
        }
    }
}
