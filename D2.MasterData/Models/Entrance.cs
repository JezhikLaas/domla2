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
            if (argument.SubUnits != null) {
                var items = from subUnitParameter in argument.SubUnits
                            select new SubUnit(subUnitParameter, this);

                _subUnits = new List<SubUnit>(items);
            }
            Version = argument.Version;
            AdministrationUnit = unit;
            AdministrationUnitId = unit.Id;
        }

        [MaxLength(256)]
        public string Title
        {
            get;
            private set;
        }

        public Address Address
        {
            get;
            private set;
        }

        public Guid AdministrationUnitId
        {
            get;
            private set;
        }

        public AdministrationUnit AdministrationUnit
        {
            get;
            private set;
        }

        private IList<SubUnit> _subUnits;
        public IEnumerable<SubUnit> SubUnits
        {
            get { return _subUnits; }
        }

        [ConcurrencyCheck]
        public int Version
        {
            get;
            private set;
        }
    }
}
