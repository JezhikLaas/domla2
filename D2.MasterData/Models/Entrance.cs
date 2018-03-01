using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace D2.MasterData.Models
{
    public class Entrance : BaseModel
    {
        Entrance()
        { }

        public Entrance(EntranceParameters argument, AdministrationUnit adm)
        {
            Id = argument.Id;
            Title = argument.Title;
            Address = new Address(argument.Address);
            if (argument.SubUnits != null) {
                var items = from SubUnitParameter in argument.SubUnits
                            select new SubUnit(SubUnitParameter, this);

                SubUnits = new List<SubUnit>(items);
            }
            AdministrationUnit = adm;
            AdministrationUnitId = adm.Id;
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

        public List<SubUnit> SubUnits
        {
            get;
            private set;
        }
    }
}
