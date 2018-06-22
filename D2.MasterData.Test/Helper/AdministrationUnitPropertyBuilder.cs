using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    class AdministrationUnitPropertyBuilder
    {
        readonly AdministrationUnitPropertyParameters _administrationUnitPropertyParameters;

        private AdministrationUnitPropertyBuilder()
        {
            _administrationUnitPropertyParameters = AdministrationUnitPropertyParameterBuilder.New.Build();

        }

        static public AdministrationUnitPropertyBuilder New
        {
            get { return new AdministrationUnitPropertyBuilder(); }
        }

        public AdministrationUnitPropertyBuilder WithId(Guid id)
        {
            _administrationUnitPropertyParameters.Id = id;
            return this;
        }

        public AdministrationUnitPropertyBuilder WithVersion(int version)
        {
            _administrationUnitPropertyParameters.Version = version;
            return this;
        }

        public AdministrationUnitPropertyBuilder WithTitle(string title)
        {
            _administrationUnitPropertyParameters.Title = title;
            return this;
        }

        public AdministrationUnitProperty Build()
        {
            var unit = AdministrationUnitBuilder.New
                .WithId(Guid.NewGuid())
                .Build();
            return new AdministrationUnitProperty(_administrationUnitPropertyParameters, unit) ;
        }
    }
}
