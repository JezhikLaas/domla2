using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D2.MasterData.Models
{
    public class Entrance
    {
        Entrance()
        { }

        public Entrance(EntranceParameters argument)
        {
            Id = argument.Id;
            Title = argument.Title;
            Address = new Address(argument.Address);
        }

        [Key]
        public Guid Id
        {
            get;
            internal set;
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

        public Guid AdministrationId
        {
            get;
            private set;
        }

        public AdministrationUnit AdministrationUnit
        {
            get;
            private set;
        }
    }
}
