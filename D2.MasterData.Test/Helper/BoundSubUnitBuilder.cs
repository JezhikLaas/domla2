using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    class BoundSubUnitBuilder
    {
        readonly BoundSubUnitParameters _boundSubUnitParameters;

        private BoundSubUnitBuilder()
        {
            _boundSubUnitParameters = BoundSubUnitParametersBuilder.New.Build();
        }

        static public BoundSubUnitBuilder New
        {
            get { return new BoundSubUnitBuilder(); }
        }

        public BoundSubUnitBuilder WithId(Guid id)
        {
            _boundSubUnitParameters.Id = id;
            return this;
        }

        public BoundSubUnitBuilder WithVersion(int version)
        {
            _boundSubUnitParameters.Version = version;
            return this;
        }

        public BoundSubUnitBuilder WithTitle(string title)
        {
            _boundSubUnitParameters.Title = title;
            return this;
        }

        public BoundSubUnit Build()
        {
            var unit = AdministrationUnitBuilder.New
                .WithId(Guid.NewGuid())
                .Build();
            var entrance = from entrances in unit.Entrances
                           select entrances;
            return new BoundSubUnit(_boundSubUnitParameters, entrance.First(), entrance.First().AdministrationUnit);
        }
    }
}
