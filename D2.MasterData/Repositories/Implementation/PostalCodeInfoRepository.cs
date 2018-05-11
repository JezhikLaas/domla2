using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.Service.IoC;
using System;
using System.Collections.Generic;
using System.Linq;


namespace D2.MasterData.Repositories.Implementation
{
    [RequestScope]
    public class PostalCodeInfoRepository : IPostalCodeInfoRepository
    {
        readonly IDataContext _context;

        public PostalCodeInfoRepository(IDataContext context)
        {
            _context = context;
        }

        public void Insert(PostalCodeInfo item)
        {
            using (var transaction = _context.Session.BeginTransaction())
            {
                _context.Session.Save(item);
                transaction.Commit();
            }
        }

        public IEnumerable<PostalCodeInfo> List()
        {
            var result = from unit in _context.Session.Query<PostalCodeInfo>()
                         orderby unit.PostalCode
                         select unit;

            return result.ToList();
        }

        public PostalCodeInfo Load(Guid id)
        {
            return _context.Session.Get<PostalCodeInfo>(id);
        }
    }
}
