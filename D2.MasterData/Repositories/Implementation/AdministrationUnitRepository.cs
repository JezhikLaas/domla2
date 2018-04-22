using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.Service.IoC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace D2.MasterData.Repositories.Implementation
{
    [RequestScope]
    public class AdministrationUnitRepository : IAdministrationUnitRepository
    {
        readonly ISession _connection;

        public AdministrationUnitRepository(ISession context)
        {
            _connection = context;
        }

        public void Insert(AdministrationUnit item)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                _connection.Save(item);
                transaction.Commit();
            }
        }

        public IEnumerable<AdministrationUnit> List()
        {
            var result = from unit in _connection.Query<AdministrationUnit>()
                         orderby unit.UserKey
                         select unit;

            return result.ToList();
        }

        public AdministrationUnit Load(Guid id)
        {
            return _connection.Get<AdministrationUnit>(id);
        }

        public void Update(AdministrationUnit administrationUnit)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                _connection.Update(administrationUnit);
                transaction.Commit();
            }
        }
    }
}
