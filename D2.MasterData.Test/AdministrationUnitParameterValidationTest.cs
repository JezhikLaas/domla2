using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using System.Linq;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitParameterValidationTest
    {
        [Fact]
        public void Validation_succeeds_for_valid_Post()
        {
            var parameters = new AdministrationUnitParameters {
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

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validation_fails_for_invalid_Post()
        {
            var parameters = new AdministrationUnitParameters {
                Address = new AddressParameters {
                    City = "H",
                    Country = new CountryInfoParameters {
                        Iso2 = "DE",
                        Name = "Deutschland",
                        Iso3 = "DEU"
                    }
                }
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Equal(2, result.Errors.Count());
        }
    }
}
