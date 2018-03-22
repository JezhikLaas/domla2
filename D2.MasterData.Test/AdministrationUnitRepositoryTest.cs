using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.MasterData.Repositories.Implementation;
using D2.MasterData.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

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
            var result = new MasterDataContext();// new TestContext(_options);
            result.Database.EnsureCreated();
            return result;
        }

        [Fact(DisplayName = "AdministrationUnitRepository can insert AdministrationUnit")]
        public void AdministrationUnitRepository_can_insert_AdministrationUnit()
        {
            var parameters = new AdministrationUnitParameters {
                UserKey = "03",
                Title = "ABC",
                Entrances = new List<EntranceParameters>
                {
                    new EntranceParameters {
                        Title = "Eingang 49",
                        Address = new AddressParameters
                        {
                            Street = "Seumestraße",
                            Number = "49",
                            PostalCode = "22222",
                            Country = new CountryInfoParameters
                            {
                                Iso2 = "DE",
                                Name = "Deutschland",
                                Iso3 = "DEU"
                            }
                        }
                    }
                }
            };
            var unit = new AdministrationUnit(parameters);

            using (var context = GetContext()) {
                var repository = new AdministrationUnitRepository(context);
                repository.Insert(unit);
            }

            using (var context = GetContext()) {
                var repository = new AdministrationUnitRepository(context);
                var stored = repository.List();

                Assert.Collection(stored, u => Assert.Equal("03", u.UserKey));
                Assert.Collection(stored, u => Assert.Collection(u.Entrances, e => Assert.Equal("Eingang 49", e.Title)));
            }
        }
    }
}
