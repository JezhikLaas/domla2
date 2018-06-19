using D2.MasterData.Parameters;
using System;
using System.ComponentModel.DataAnnotations;

namespace D2.MasterData.Models
{
    public class SubUnit : BaseModel
    {
        protected SubUnit()
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

        public virtual string Title
        {
            get;
            protected set;
        }

        public virtual Guid EntranceId
        {
            get;
            protected set;
        }

        public virtual Entrance Entrance
        {
            get;
            protected set;
        }

        public virtual int? Floor
        {
            get;
            protected set;
        }

        public virtual int? Number
        {
            get;
            protected set;
        }

        public virtual string Usage
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
