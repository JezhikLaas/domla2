using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using D2.Common;

namespace D2.MasterData.Test.Helper
{
    class AdministrationUnitBuilder
    {
        readonly AdministrationUnitParameters _administrationUnitParameters;

        private AdministrationUnitBuilder()
        {
            _administrationUnitParameters = new AdministrationUnitParameters {
                UserKey = "03",
                Title = "ABC",
                YearOfConstruction = new YearMonth(2001, 10),
                Entrances = new List<EntranceParameters>
                {
                    new EntranceParameters {
                        Title = "Eingang 49",
                        Address = new AddressParameters
                        {
                            Street = "Seumestraße",
                            Number = "49",
                            PostalCode = "22222",
                            City = "Herford",
                            Country = new CountryInfoParameters
                            {
                                Iso2 = "DE",
                                Name = "Deutschland"
                            }
                        }
                    }
                }
            };
        }

        static public AdministrationUnitBuilder New
        {
            get { return new AdministrationUnitBuilder(); }
        }

        public AdministrationUnitBuilder WithId(Guid id)
        {
            _administrationUnitParameters.Id = id;
            return this;
        }

        public AdministrationUnitBuilder WithTitle(string title)
        {
            _administrationUnitParameters.Title = title;
            return this;
        }

        public AdministrationUnitBuilder WithVersion(int version)
        {
            _administrationUnitParameters.Version = version;
            return this;
        }

        public AdministrationUnit Build()
        {
            return new AdministrationUnit(_administrationUnitParameters);
        }
    }
}
