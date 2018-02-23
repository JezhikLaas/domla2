﻿using D2.MasterData.Models;
using D2.MasterData.Parameters;
using Newtonsoft.Json;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitSerializationTest
    {
        [Fact(DisplayName = "Serialize and deserialize AdministrationUnitParameters")]
        public void Serialize_and_deserialize_AdministrationUnitParameters()
        {
            var test = new AdministrationUnitParameters
            {
                Title = "ABC",
                UserKey = "02",
                Address = new AddressParameters {
                    City = "H",
                    Country = new CountryInfoParameters {
                        Iso2 = "DE",
                        Name = "Deutschland",
                        Iso3 = "DEU"
                    }
                }
            };

            var text = JsonConvert.SerializeObject(test);
            var check = JsonConvert.DeserializeObject<AdministrationUnitParameters>(text);

            Assert.Equal(test.Title, check.Title);
            Assert.Equal(test.UserKey, check.UserKey);
            Assert.Equal(test.Address.City, check.Address.City);
            Assert.Equal(test.Address.Country.Iso2, check.Address.Country.Iso2);
            Assert.Equal(test.Address.Country.Iso3, check.Address.Country.Iso3);
            Assert.Equal(test.Address.Country.Name, check.Address.Country.Name);
        }
    }
}
