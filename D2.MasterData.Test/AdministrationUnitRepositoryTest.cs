using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.MasterData.Repositories.Implementation;
using D2.MasterData.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using D2.MasterData.Test.Helper;
using System.Linq;
using System.Reflection;

namespace D2.MasterData.Test
{
    public class AdministrationUnitRepositoryTest : IDisposable
    {
        DbContextOptions<TestContext> _options;
        SqliteConnection _connection;

        public class TestContext : MasterDataContext
        {
            public TestContext(DbContextOptions<TestContext> options)
                : base(options)
            { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                OnCommonModelCreating(modelBuilder);
            }
        }

        public AdministrationUnitRepositoryTest()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<TestContext>()
                .UseSqlite(_connection)
                .Options;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing) {
                _connection.Dispose();
            }
        }

        MasterDataContext GetContext()
        {
            var result = new TestContext(_options);
            result.Database.EnsureCreated();
            return result;
        }

        Guid InsertAdministrationUnit()
        {
            var unit = AdministrationUnitBuilder.New.Build();

            using (var context = GetContext()) {
                var repository = new AdministrationUnitRepository(context);
                repository.Insert(unit);
            }

            return unit.Id;
        }

        [Fact(DisplayName = "AdministrationUnitRepository can insert AdministrationUnit")]
        public void AdministrationUnitRepository_can_insert_AdministrationUnit()
        {
            InsertAdministrationUnit();

            using (var context = GetContext()) {
                var repository = new AdministrationUnitRepository(context);
                var stored = repository.List();

                Assert.Collection(stored, u => Assert.Equal("03", u.UserKey));
                Assert.Collection(stored, u => Assert.Collection(u.Entrances, e => Assert.Equal("Eingang 49", e.Title)));
            }
        }

        [Fact(DisplayName = "AdministrationUnitRepository yields null for unknown id")]
        public void AdministrationUnitRepository_yields_null_for_unknown_id()
        {
            InsertAdministrationUnit();

            using (var context = GetContext()) {
                var repository = new AdministrationUnitRepository(context);
                Assert.Null(repository.Load(Guid.NewGuid()));
            }
        }

        [Fact(DisplayName = "AdministrationUnitRepository can update AdministrationUnit")]
        public void AdministrationUnitRepository_can_update_AdministrationUnit()
        {
            var id = InsertAdministrationUnit();
            var unit = AdministrationUnitBuilder.New.WithId(id).WithTitle("Drachenhöhle").Build();

            using (var context = GetContext())
            {
                var repository = new AdministrationUnitRepository(context);
                repository.Update(unit);
            }
            using (var context = GetContext())
            {
                var repository = new AdministrationUnitRepository(context);
                var modified = repository.Load(id);
                Assert.Equal("Drachenhöhle", modified.Title);
            }
        }
    }
}