using D2.MasterData.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Parameters
{
    public class SelectedAdministrationUnitPropertyParameters
    {
        [NotNullOrEmpty]
        public AdministrationUnitsFeatureParameters AdministrationUnitsFeatureParameters { get; set; }

        [NotNullOrEmpty]
        public Guid [] AdministrationUnitIds { get; set; }

    }
}
