using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    class AdministrationUnitBuilder
    {
        AdministrationUnitParameters _administrationUnitParameters;

        private AdministrationUnitBuilder()
        {
            _administrationUnitParameters = new AdministrationUnitParameters {
                UserKey = "03",
                Title = "ABC",
                Entrances = new List<EntranceParameters>
                {
                    new EntranceParameters {
                        Title = "Eingang 49",
                        Address = new AddressParameters
                        {
                            Street = "Seumestraße",
                            Number = "49",
                            PostalCode = "22222",
                            Country = new CountryInfoParameters
                            {
                                Iso2 = "DE",
                                Name = "Deutschland",
                                Iso3 = "DEU"
                            }
                        }
                    }
                }
            };
        }

        static public AdministrationUnitBuilder New => new AdministrationUnitBuilder();

        public AdministrationUnit Build()
        {
            return new AdministrationUnit(_administrationUnitParameters);
        }
    }
}
