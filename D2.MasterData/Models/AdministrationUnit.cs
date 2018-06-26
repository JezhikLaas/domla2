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
        }

        public AdministrationUnit(AdministrationUnitParameters argument)
        {
            Id = argument.Id;
            UserKey = argument.UserKey;
            Title = argument.Title;
            YearOfConstruction = argument.YearOfConstruction;
            Version = argument.Version;

            var items = from entranceParameter in argument.Entrances
                        select new Entrance(entranceParameter, this);
            _entrances = new List<Entrance>(items);

            _administrationUnitProperties = new List<AdministrationUnitProperty>();

            if (argument.AdministrationUnitProperties != null)
            {
                var properties = from propertiesParameter in argument.AdministrationUnitProperties
                                 select new AdministrationUnitProperty(propertiesParameter, this);
                _administrationUnitProperties = new List<AdministrationUnitProperty>(properties);
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
