using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace D2.MasterData.Test.Helper
{
    class AdministrationUnitParametersBuilder
    {
        private List<EntranceParameters> _entrances;

        private AdministrationUnitParametersBuilder()
        {
            _entrances = new List<EntranceParameters>()
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
            };
        }

        static public AdministrationUnitParametersBuilder New
        {
            get { return new AdministrationUnitParametersBuilder(); }
        }

        public AdministrationUnitParameters Build()
        {
            return new AdministrationUnitParameters
            {
                Entrances = _entrances
            };
        }
    }
}
