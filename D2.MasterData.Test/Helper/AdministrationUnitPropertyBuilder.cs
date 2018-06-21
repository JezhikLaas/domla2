using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    class AdministrationUnitPropertyBuilder
    {
        readonly AdministrationUnitPropertyParameters _AdministrationUnitPropertyParameters;

        private AdministrationUnitPropertyBuilder()
        {
            _AdministrationUnitPropertyParameters = AdministrationUnitPropertyParameterBuilder.New.Build();

        }

        static public AdministrationUnitPropertyBuilder New
        {
            get { return new AdministrationUnitPropertyBuilder(); }
        }

        public AdministrationUnitPropertyBuilder WithId(Guid id)
        {
            _AdministrationUnitPropertyParameters.Id = id;
            return this;
        }

        public AdministrationUnitPropertyBuilder WithVersion(int version)
        {
            _AdministrationUnitPropertyParameters.Version = version;
            return this;
        }

        public AdministrationUnitPropertyBuilder WithTitle(string title)
        {
            _AdministrationUnitPropertyParameters.Title = title;
            return this;
        }
    }
}
