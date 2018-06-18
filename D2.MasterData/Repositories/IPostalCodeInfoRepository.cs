using D2.MasterData.Models;
using D2.MasterData.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D2.MasterData.Repositories
{
    public interface IPostalCodeInfoRepository
    {
        void Insert(PostalCodeInfo item);
        IEnumerable<PostalCodeInfo> List();
        PostalCodeInfo Load(Guid id);
        bool Exists(PostalCodeInfoParameters postalcode);
    }
}
