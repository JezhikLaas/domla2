using D2.MasterData.Parameters;
using System;
using System.ComponentModel.DataAnnotations;

namespace D2.MasterData.Models
{
    public class SubUnit : BaseModel
    {
        private SubUnit()
        { }

        public SubUnit(SubUnitParameters argument, Entrance entrance)
        {
            Id = argument.Id;
            Title = argument.Title;
            Floor = argument.Floor;
            Number = argument.Number;
            Usage = argument.Usage;
            Version = argument.Version;
            Entrance = entrance;
            EntranceId = entrance.Id;
        }


        [MaxLength(256)]
        public string Title
        {
            get;
            private set;
        }

        public Guid EntranceId
        {
            get;
            private set;
        }

        [Required]
        public Entrance Entrance
        {
            get;
            private set;
        }

        public int? Floor
        {
            get;
            private set;
        }

        [Required]
        public int? Number
        {
            get;
            private set;
        }

        [MaxLength(256)]
        public string Usage
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
