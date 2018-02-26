using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace D2.MasterData.Models
{
    public class AdministrationUnit
    {
        AdministrationUnit()
        { }

        public AdministrationUnit(AdministrationUnitParameters argument)
        {
            Id = argument.Id;
            UserKey = argument.UserKey;
            Title = argument.Title;
            Entrances = new List<Entrance>(argument.Entrances.Select(etr => new Entrance(etr)));
        }

        [Key]
        public Guid Id
        {
            get;
            internal set;
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

        public Guid EntranceID 
        {
            get;
            set;
        }

        public List<Entrance> Entrances
        {
            get;
            private set;
        }
    }
}
