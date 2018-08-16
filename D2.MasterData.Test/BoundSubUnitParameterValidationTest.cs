using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using Xunit;

namespace D2.MasterData.Test
{
    public class ApartmentParameterValidationTest
    {
        [Fact(DisplayName = "Validation of BoundSubUnitParameters succeeds for valid post")]
        public void Validation_of_SubUnitParameters_succeeds_for_valid_post()
        {
            var parameters = new BoundSubUnitParameters
            {
                Title = "Apartment 1",
                Floor = "EG",
                Number = 1,
                Usage = "Wohnung"

            };            

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validation of SubUnitParameters fails for invalid post")]
        public void Validation_of_SubUnitParameters_fails_for_invalid_post()
        {
            var parameters = new BoundSubUnitParameters
            {
                Number = 1
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
        }

        [Fact(DisplayName = "Validation of SubUnitParameters for post succeeds w/o floor")]
        public void Validation_of_SubUnitParameters_for_post_succeeds_wo_floor()
        {
            var parameters = new BoundSubUnitParameters
            {
                Title = "ABC",
                Number = 1
            };

            var validator = new ParameterValidator();

            var result = validator.Validate(parameters, RequestType.Post);
            Assert.True(result.IsValid);
        }

    }
}
