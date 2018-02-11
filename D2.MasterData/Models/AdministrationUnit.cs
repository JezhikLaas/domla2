using D2.MasterData.Parameters;
using System;

namespace D2.MasterData.Models
{
    public class AdministrationUnit
    {
        public AdministrationUnit(AdministrationUnitParameters argument)
        {
            var validation = argument.Validate();
            if (validation.IsValid == false) throw new ArgumentException("parameters are invalid");

            UserKey = argument.UserKey;
            Title = argument.Title;
            Address = argument.Address;
        }

        public Guid Id { get; }
        public string UserKey { get; set; }
        public string Title { get; set; }
        public Address Address { get; set; }
    }
}
