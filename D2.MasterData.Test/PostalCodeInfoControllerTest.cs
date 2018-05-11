using D2.MasterData.Controllers;
using D2.MasterData.Facades;
using NSubstitute;
using Xunit;

namespace D2.MasterData.Test
{
    public class PostalCodeInfoControllerTest
    {
        [Fact(DisplayName = "PostalCodeInfoController.Post calls CreateNewPostalCodeInfo")]
        public void PostalCodeInfoController_Post_calls_CreateNewPostalCodeInfo()
        {
            var facade = Substitute.For<IPostalCodeInfoFacade>();
            var controller = new PostalCodeInfoController(facade);

            controller.Create(null);
            facade.Received(1).CreateNewPostalCodeInfo(null);
        }
    }
}
