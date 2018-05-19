using D2.MasterData.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Parameters
{
    public class PostalCodeInfoParameters
    {
        public Guid Id { get; set; }

        [NotNullOrEmpty]
        public string Iso2 { get; set; }

        [NotNullOrEmpty]
        public string PostalCode { get; set; }

        [NotNullOrEmpty]
        public string City { get; set; }

        public int Version { get; set; }
    }
}
