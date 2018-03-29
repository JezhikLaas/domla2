using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitParameterValidationTest
    {
        [Fact(DisplayName = "Validation of AdministrationUnitParameters succeeds for valid post")]
        public void Validation_of_AdministrationUnitParameters_succeeds_for_valid_post()
        {
            var parameters = new AdministrationUnitParameters {
                Title = "ABC",
                UserKey = "02",
                Entrances = new List<EntranceParameters> {
                    new EntranceParameters {
                        Title = "Seumestraße 49",
                        Address = new AddressParameters{
                            Street = "Seumestraße",
                            Number = "49",
                            PostalCode = "22222",
                            City = "Hamburg",
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

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validation of AdministrationUnitParameters fails for invalid post")]
        public void Validation_of_AdministrationUnitParameters_fails_for_invalid_post()
        {
            var parameters = new AdministrationUnitParameters {
                Entrances = new List<EntranceParameters> {
                    new EntranceParameters {
                        Address = new AddressParameters {
                            City = "H",
                            Country = new CountryInfoParameters
                            {
                                Iso2 = "DE",
                                Name = "Deutschland",
                                Iso3 = "DEU"
                            }
                        }
                    }
                },
                Title = "Eingang1"
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Equal(5, result.Errors.Count());
        }

        [Fact(DisplayName = "Validation of AdministrationUnitParameters for post succeeds w/o YearOfConstuction")]
        public void Validation_of_AdministrationUnitParameters_for_post_succeeds_wo_YearOfConstuction()
        {
            var parameters = new AdministrationUnitParameters {
                Title = "ABC",
                UserKey = "02",
                Entrances = new List<EntranceParameters> {
                    new EntranceParameters {
                        Address = new AddressParameters {
                            City = "H",
                            Country = new CountryInfoParameters
                            {
                                Iso2 = "DE",
                                Name = "Deutschland",
                                Iso3 = "DEU"
                            },
                            Number = "1",
                            PostalCode = "12345",
                            Street = "Testweg"
                        },
                        Title = "Eingang1"
                    }
                },

            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validation of AdministrationUnitParameters for put fails w/o entrance")]
        public void Validation_of_AdministrationUnitParameters_for_put_fails_wo_entrance()
        {
            var parameters = new AdministrationUnitParameters {
                Title = "ABC",
                UserKey = "02"
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Put);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Validation of AdministrationUnitParameters for post fails w/o entrance")]
        public void Validation_of_AdministrationUnitParameters_for_post_fails_wo_entrance()
        {
            var parameters = new AdministrationUnitParameters {
                Title = "ABC",
                UserKey = "02"
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Validation of AdministrationUnitParameters for put fails with invalid address")]
        public void Validation_of_AdministrationUnitParameters_for_put_fails_with_invalid_address()
        {
            var parameters = new AdministrationUnitParameters {
                Title = "ABC",
                UserKey = "02",
                Entrances = new List<EntranceParameters> {
                    new EntranceParameters {
                        Address = new AddressParameters
                        {
                            // Address w/o street is invalid
                            City = "H",
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
            
            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Put);
            Assert.False(result.IsValid);
            Assert.Equal(4, result.Errors.Count());
        }

        [Fact(DisplayName = "Validation of AdministrationUnitParameters for post fails with null")]
        public void Validation_of_AdministrationUnitParameters_for_post_fails_with_null()
        {
            var validator = new ParameterValidator();

            var result = validator.Validate(null, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Validation of AdministrationUnitParameters with Entrance w/o Address for post fails")]
        public void Validation_of_AdministrationUnitParameters_with_Entrance_wo_Address_for_post_fails()
        {
            var parameters = new AdministrationUnitParameters {
                Title = "ABC",
                UserKey = "02",
                Entrances = new List<EntranceParameters> {
                    new EntranceParameters {
                        Address = null,
                        Title = "XXX"
                    }
                }
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }
    }
}
