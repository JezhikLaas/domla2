using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace D2.MasterData.Test
{
    public class PostalCodeInfoParameterValidationTest
    {
        [Fact(DisplayName = "Validation of PostalCodeInfoParameters succeeds for valid post")]
        public void Validation_of_PostalCodeInfoParameters_succeeds_for_valid_post()
        {
            var parameters = new PostalCodeInfoParameters
            {
                PostalCode = "32051",
                City = "Herford",
                Iso2 = "DE"
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.True(result.IsValid);
        }


        [Fact(DisplayName = "Validation of PostalCodeInfoParameters for post fails w/o City")]
        public void Validation_of_PostalCodeInfoParameters_for_post_fails_wo_city()
        {
            var parameters = new PostalCodeInfoParameters
            {
                PostalCode = "32602",
                Iso2 = "DE"
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Validation of PostalCodeInfoParameters for post fails w/o Iso2")]
        public void Validation_of_PostalCodeInfoParameters_for_post_fails_wo_iso2()
        {
            var parameters = new PostalCodeInfoParameters
            {
                PostalCode = "32602",
                City = "Herford"
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Validation of PostalCodeInfoParameters for post fails w/o PostalCode")]
        public void Validation_of_PostalCodeInfoParameters_for_post_fails_wo_postalcode()
        {
            var parameters = new PostalCodeInfoParameters
            {
                City = "Herford",
                Iso2 = "DE"
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Validation of PostalCodeInfoParameters for post fails with invalid PostalCode")]
        public void Validation_of_PostalCodeInfoParameters_for_post_fails_with_invalid_postalcode()
        {
            var parameters = new PostalCodeInfoParameters
            {
                PostalCode = "ABC",
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Put);
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Validation of PostalCodeInfoParameters for post fails with null")]
        public void Validation_of_PostalCodeInfoParameters_for_post_fails_with_null()
        {
            var validator = new ParameterValidator();

            var result = validator.Validate(null, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }
    }
}
