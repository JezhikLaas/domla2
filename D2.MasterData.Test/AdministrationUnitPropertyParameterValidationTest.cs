using D2.Infrastructure;
using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitPropertyParameterValidationTest
    {
        [Fact(DisplayName = "Validation of AdministrationUnitPropertyParameters succeeds for valid post")]
        public void ValidationOfAdministrationUnitPropertyParameters_succeeds_for_valid_post()
        {
            var parameters = new AdministrationUnitPropertyParameters
            {
                Title = "Wohnflaeche",
                Description = "Wird benoetigt",
                Value = new Variant (new TypedValue(200,"meter",2))
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.True(result.IsValid);
        }


        [Fact(DisplayName = "Validation of AdministrationUnitPropertyParameters for post fails w/o Titel")]
        public void Validation_of_AdministrationUnitPropertyParameters_for_post_fails_wo_Titel()
        {
            var parameters = new AdministrationUnitPropertyParameters
            {
                Description = "Wird benoetigt",
                Value = new Variant(new TypedValue(200, "meter", 2))
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Validation of AdministrationUnitPropertyParameters for post fails w/o Value")]
        public void ValidationOfAdministrationUnitPropertyParameters_succeeds_for_Post_fails_wo_Value()
        {
            var parameters = new AdministrationUnitPropertyParameters
            {
                Title = "Wohnflaeche",
                Description = "Wird benoetigt"
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Validation of AdministrationUnitPropertyParameters for post fails with null")]
        public void Validation_of_AdministrationUnitPropertyParameters_for_post_fails_with_null()
        {
            var validator = new ParameterValidator();

            var result = validator.Validate(null, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }
    }
}
