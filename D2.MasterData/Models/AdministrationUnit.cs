using System;

namespace D2.MasterData.Models
{
    public class AdministrationUnit
    {
        public Guid Id { get; }
        public string UserKey { get; set; }
        public string Title { get; set; }
        public Address Address { get; set; }
    }
}
