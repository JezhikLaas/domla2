using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using D2.Common;

namespace D2.MasterData.Models
{
    public class AdministrationUnit : BaseModel
    {
        protected AdministrationUnit()
        {
            _entrances = new List<Entrance>();
            _administrationUnitProperties = new List<AdministrationUnitProperty>();
            _unboundSubUnits = new List<UnboundSubUnit>();
        }

        public AdministrationUnit(AdministrationUnitParameters argument)
        {
            Id = argument.Id;
            UserKey = argument.UserKey;
            Title = argument.Title;
            var items = from entranceParameter in argument.Entrances
                        select new Entrance(entranceParameter, this);
            _entrances = new List<Entrance>(items);
            _administrationUnitProperties = new List<AdministrationUnitProperty>();
            YearOfConstruction = argument.YearOfConstruction;
            Version = argument.Version;
            if (argument.AdministrationUnitProperties != null)
            {
                var properties = from propertiesParameter in argument.AdministrationUnitProperties
                                 select new AdministrationUnitProperty(propertiesParameter, this);
                _administrationUnitProperties = new List<AdministrationUnitProperty>(properties);
            }
            if (argument.UnboundSubUnits != null)
            {
                var unboundSubUnits = from subUnitParameter in argument.UnboundSubUnits
                                        select new UnboundSubUnit(subUnitParameter, this);
                _unboundSubUnits = new List<UnboundSubUnit>(unboundSubUnits);
            }
        }

        public virtual string UserKey
        {
            get;
            protected set;
        }

        public virtual string Title
        {
            get;
            protected set;
        }

        private IList<Entrance> _entrances;

        public virtual IEnumerable<Entrance> Entrances
        {
            get { return _entrances; }
        }

        private IList<UnboundSubUnit> _unboundSubUnits;

        public virtual IEnumerable<UnboundSubUnit> UnboundSubUnits
        {
            get { return _unboundSubUnits; }
        }

        public virtual IEnumerable<SubUnit> SubUnits
        {
            get {
                var subUnits = _entrances.SelectMany(x => x.SubUnits).Cast<SubUnit>();
                return subUnits.Concat(_unboundSubUnits.Cast<SubUnit>());
            }
        }
        private IList<AdministrationUnitProperty> _administrationUnitProperties;

        public virtual IEnumerable<AdministrationUnitProperty> AdministrationUnitProperties
        {
            get { return _administrationUnitProperties; }
        }

        public virtual YearMonth? YearOfConstruction
        {
            get;
            protected set;
        }

        public virtual int Version
        {
            get;
            protected set;
        }

        public virtual void AddProperty (AdministrationUnitProperty property)
        {
            _administrationUnitProperties.Add(property);
        }
    }
}
