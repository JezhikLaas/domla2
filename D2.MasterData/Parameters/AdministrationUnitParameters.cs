using D2.MasterData.Controllers.Validators;
using D2.MasterData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Parameters
{
    public abstract class AdministrationUnitParameters
    {
        public Guid Id { get; set; }
        public string UserKey { get; set; }
        public string Title { get; set; }
        public Address Address { get; set; }

        public abstract ValidationResult Validate();
    }

    public class AdministrationUnitPostParameters : AdministrationUnitParameters
    {
        public override ValidationResult Validate()
        {
            return null;
        }
    }
}
