using D2.MasterData.Controllers;
using D2.MasterData.Facades;
using NSubstitute;
using Xunit;

namespace D2.MasterData.Test
{
    public class AdministrationUnitControllerTest
    {
        [Fact]
        public void When_calling_post_CreateNewAdministrationUnit_is_invoked()
        {
            var facade = Substitute.For<IAdministrationUnitFacade>();
            var controller = new AdministrationUnitController(facade);

            controller.Post(null);
            facade.Received(1).CreateNewAdministrationUnit(null);
        }
    }
}
