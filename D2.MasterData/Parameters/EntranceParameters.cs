using D2.MasterData.Infrastructure.Validation;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Parameters
{
    public class EntranceParameters
    {
        public Guid Id { get; set; }

        [NotNullOrEmpty]
        public string Title { get; set; }

        [NotNullOrEmpty]
        public AddressParameters Address { get; set; }

        public int Version { get; set; }

        public List<BoundSubUnitParameters> SubUnits { get; set; }
    }
}
