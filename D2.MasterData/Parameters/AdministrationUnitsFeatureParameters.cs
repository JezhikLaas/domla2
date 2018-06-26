using D2.Infrastructure;
using D2.MasterData.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Parameters
{
    public class AdministrationUnitsFeatureParameters
    {
        public Guid Id { get; set; }

        [NotNullOrEmpty]
        public string Title { get; set; }

        public string Description { get; set; }

        [NotNullOrEmpty]
        public VariantTag Tag { get; set; }

        public int TypedValueDecimalPlace { get; set; }

        public string TypedValueUnit { get; set; }

        public int Version { get; set; }

    }
}
