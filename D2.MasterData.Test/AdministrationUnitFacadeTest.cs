using D2.MasterData.Facades.Implementation;
using D2.MasterData.Infrastructure;
using D2.MasterData.Repositories;
using D2.MasterData.Test.Helper;
using NSubstitute;
using System;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitFacadeTest
    {
        [Fact(DisplayName = "Load with invalid id yields 422")]
        public void Load_with_invalid_id_yields_422()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IAdministrationUnitRepository>();

            var facade = new AdministrationUnitFacade(repository, validator);
            var result = facade.LoadAdministrationUnit("");

            Assert.Equal(422, result.code);
        }

        [Fact(DisplayName = "Load with unknown id yields 404")]
        public void Load_with_unknown_id_yields_404()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IAdministrationUnitRepository>();

            var facade = new AdministrationUnitFacade(repository, validator);
            var result = facade.LoadAdministrationUnit(Guid.NewGuid().ToString());

            Assert.Equal(404, result.code);
        }

        [Fact(DisplayName = "Load with known id yields 200 and valid json object")]
        public void Load_with_known_id_yields_200_and_valid_json_object()
        {
            var validator = Substitute.For<IParameterValidator>();
            var repository = Substitute.For<IAdministrationUnitRepository>();
            repository.Load(Arg.Any<Guid>()).Returns(AdministrationUnitBuilder.New.Build());

            var facade = new AdministrationUnitFacade(repository, validator);
            var result = facade.LoadAdministrationUnit(Guid.NewGuid().ToString());

            Assert.Equal(200, result.code);
            Assert.False(string.IsNullOrWhiteSpace(result.json));
        }
    }
}
