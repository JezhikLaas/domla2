using D2.MasterData.Facades.Implementation;
using D2.MasterData.Infrastructure;
using D2.MasterData.Parameters;
using D2.MasterData.Repositories;
using D2.MasterData.Test.Helper;
using D2.Service.Contracts.Validation;
using NSubstitute;
using System;
using Xunit;

namespace D2.MasterData.Test
{
    public class PostalCodeInfoFacadeTest
    {
        [Fact(DisplayName = "Load with invalid id yields 422")]
        public void Load_with_invalid_id_yields_422()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IPostalCodeInfoRepository>();

            var facade = new PostalCodeInfoFacade(repository, validator);
            var result = facade.LoadPostalCodeInfo("");

            Assert.Equal(422, result.code);
        }

        [Fact(DisplayName = "Load with unknown id yields 404")]
        public void Load_with_unknown_id_yields_404()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IPostalCodeInfoRepository>();

            var facade = new PostalCodeInfoFacade(repository, validator);
            var result = facade.LoadPostalCodeInfo(Guid.NewGuid().ToString());

            Assert.Equal(404, result.code);
        }

        [Fact(DisplayName = "Load with known id yields 200 and valid json object")]
        public void Load_with_known_id_yields_200_and_valid_json_object()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IPostalCodeInfoRepository>();
            var unitId = Guid.NewGuid();

            repository.Load(Arg.Any<Guid>()).Returns(PostalCodeInfoBuilder.New.WithId(unitId).Build());

            var facade = new PostalCodeInfoFacade(repository, validator);
            var result = facade.LoadPostalCodeInfo(unitId.ToString());

            Assert.Equal(200, result.code);
            Assert.False(string.IsNullOrWhiteSpace(result.json));
        }

        [Fact(DisplayName = "ValidateCreate with invalid object yields ExternalFailure")]
        public void ValidateCreate_with_invalid_object_yields_ExternalFailure()
        {
            var validator = Substitute.For<IParameterValidator>();
            validator
                .Validate(Arg.Any<PostalCodeInfoParameters>(), RequestType.Post)
                .Returns(info => {
                    var validation = new ValidationResult();
                    validation.AddError("test", "Fehler");
                    return validation;
                });
            var repository = Substitute.For<IPostalCodeInfoRepository>();
            var facade = new PostalCodeInfoFacade(repository, validator);

            var result = facade.ValidateCreate(new PostalCodeInfoParameters());
            Assert.Equal(State.ExternalFailure, result.result);
        }

        [Fact(DisplayName = "ValidateCreate with valid object yields NoError")]
        public void ValidateCreate_with_valid_object_yields_NoError()
        {
            var validator = Substitute.For<IParameterValidator>();
            validator
                .Validate(Arg.Any<PostalCodeInfoParameters>(), RequestType.Post)
                .Returns(new ValidationResult());
            var repository = Substitute.For<IPostalCodeInfoRepository>();
            var facade = new PostalCodeInfoFacade(repository, validator);

            var result = facade.ValidateCreate(new PostalCodeInfoParameters());
            Assert.Equal(State.NoError, result.result);
        }
    }
}
