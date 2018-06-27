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
            _subUnits = new List<SubUnit>();
        }

        public Entrance(EntranceParameters argument, AdministrationUnit unit)
        {
            Id = argument.Id;
            Title = argument.Title;
            Address = new Address(argument.Address);
            Version = argument.Version;
            AdministrationUnit = unit;
            AdministrationUnitId = unit.Id;
            
            _subUnits = new List<SubUnit>();
            if (argument.SubUnits != null) {
                var items = from subUnitParameter in argument.SubUnits
                            select new SubUnit(subUnitParameter, this);

                _subUnits = new List<SubUnit>(items);
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

        public virtual Guid AdministrationUnitId
        {
            get;
            protected set;
        }

        public virtual AdministrationUnit AdministrationUnit
        {
            get;
            protected set;
        }

        private IList<SubUnit> _subUnits;
        public virtual IEnumerable<SubUnit> SubUnits
        {
            get { return _subUnits; }
        }

        public virtual int Version
        {
            get;
            protected set;
        }
    }
}
