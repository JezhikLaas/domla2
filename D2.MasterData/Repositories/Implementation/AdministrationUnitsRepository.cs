﻿using D2.MasterData.Infrastructure;
using D2.MasterData.Models;
using D2.Service.IoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D2.MasterData.Repositories.Implementation
{
    [RequestScope]
    public class AdministrationUnitsRepository : IAdministrationUnitsRepository
    {
        readonly IDataContext _context;

        public AdministrationUnitsRepository(IDataContext context)
        {
            _context = context;
        }

        public void Insert(AdministrationUnit item)
        {
            using (var transaction = _context.Session.BeginTransaction())
            {
                _context.Session.Save(item);
                transaction.Commit();
            }
        }

        public IEnumerable<AdministrationUnit> List()
        {
            var result = from unit in _context.Session.Query<AdministrationUnit>()
                         orderby unit.UserKey
                         select unit;

            return result.ToList();
        }

        public AdministrationUnit Load(Guid id)
        {
            return _context.Session.Get<AdministrationUnit>(id);
        }

        public void Modify(AdministrationUnit administrationUnit)
        {
            using (var transaction = _context.Session.BeginTransaction())
            {
                _context.Session.Update(administrationUnit);
                transaction.Commit();
            }
        }

        public void Update(AdministrationUnit administrationUnit)
        {

            using (var transaction = _context.Session.BeginTransaction())
            {

                if (_context.Session.Contains(administrationUnit))
                {

                    _context.Session.Update(administrationUnit);

                }

                else
                {

                    _context.Session.Merge(administrationUnit);

                }

                transaction.Commit();

            }

        }
    }
}
