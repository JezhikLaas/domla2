using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace D2.MasterData.Models
{
    public class AdministrationUnit : BaseModel
    {
        AdministrationUnit()
        { }

        public AdministrationUnit(AdministrationUnitParameters argument)
        {
            Id = argument.Id;
            UserKey = argument.UserKey;
            Title = argument.Title;
            var items = from entranceParameter in argument.Entrances
                        select new Entrance(entranceParameter, this);
            Entrances = new List<Entrance>(items);
            YearOfConstuction = argument.YearOfConstuction;
            Version = argument.Version;
        }

        public void Update(AdministrationUnit other)
        {
            UserKey = other.UserKey;
            Title = other.Title;
        }

        [Required]
        [MaxLength(10)]
        public string UserKey
        {
            get;
            private set;
        }

        [MaxLength(256)]
        public string Title
        {
            get;
            private set;
        }

        [Required]
        public List<Entrance> Entrances
        {
            get;
            private set;
        }

        public DateTime? YearOfConstuction
        {
            get;
            private set;
        }

        [ConcurrencyCheck]
        public uint Version
        {
            get;
            private set;
        }
    }
}
