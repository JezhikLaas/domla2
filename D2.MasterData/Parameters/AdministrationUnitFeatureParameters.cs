using D2.Infrastructure;
using D2.MasterData.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Parameters
{
    public class AdministrationUnitFeatureParameters
    {
        public Guid Id { get; set; }

        [NotNullOrEmpty]
        public string Title { get; set; }

        [NotNullOrEmpty]
        public string Description { get; set; }

        [NotNullOrEmpty]
        public VariantTag Tag { get; set; }

        public int TypedValueDecimalPlace { get; set; }

        public string TypedValueUnit { get; set; }

    }
}
