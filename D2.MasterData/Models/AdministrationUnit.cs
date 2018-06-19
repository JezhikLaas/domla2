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
        }

        public AdministrationUnit(AdministrationUnitParameters argument)
        {
            Id = argument.Id;
            UserKey = argument.UserKey;
            Title = argument.Title;
            var items = from entranceParameter in argument.Entrances
                        select new Entrance(entranceParameter, this);
            _entrances = new List<Entrance>(items);
            YearOfConstruction = argument.YearOfConstruction;
            Version = argument.Version;
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
    }
}
