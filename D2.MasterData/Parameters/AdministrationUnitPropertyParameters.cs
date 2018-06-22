using D2.Infrastructure;
using D2.MasterData.Infrastructure;
using D2.MasterData.Infrastructure.Validation;
using D2.MasterData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Parameters
{
    public class AdministrationUnitPropertyParameters
    {
        public Guid Id { get; set; }

        [NotNullOrEmpty]
        public string Title { get; set; }

        public string Description { get; set; }

        [NotNullOrEmpty]
        public Variant Value { get; set; }

        public int Version { get; set; }
    }
}
