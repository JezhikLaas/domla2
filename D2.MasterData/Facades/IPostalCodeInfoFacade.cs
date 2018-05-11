using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.Service.Contracts.Execution;
using D2.Service.Contracts.Validation;
using System.Collections.Generic;

namespace D2.MasterData.Facades
{
    public interface IPostalCodeInfoFacade
    {
        void CreateNewPostalCodeInfo(PostalCodeInfoParameters value);
        IEnumerable<PostalCodeInfo> ListPostalCodeInfos();
        ExecutionResponse LoadPostalCodeInfo(string id);
        ValidationResponse ValidateCreate(PostalCodeInfoParameters value);
    }
}
