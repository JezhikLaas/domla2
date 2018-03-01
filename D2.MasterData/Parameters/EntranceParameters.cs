using D2.MasterData.Infrastructure;
using D2.MasterData.Infrastructure.Validation;
using D2.MasterData.Models;
using System;
using System.Collections.Generic;

namespace D2.MasterData.Parameters
{
    public class EntranceParameters
    {
        public Guid Id { get; set; }

        [NotNullOrEmpty]
        public string Title { get; set; }

        [NotNullOrEmpty(RequestType.Put)]
        public AddressParameters Address { get; set; }

        public List<SubUnitParameters> SubUnits { get; set; }

    }
}
