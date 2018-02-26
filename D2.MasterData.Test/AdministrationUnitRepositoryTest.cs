using D2.MasterData.Models;
using D2.MasterData.Parameters;
using D2.MasterData.Repositories.Implementation;
using D2.MasterData.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace D2.MasterData.Test
{
    public class AdministrationUnitRepositoryTest
    {
        DbContextOptions<MasterDataContext> _options;

        public AdministrationUnitRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<MasterDataContext>()
                .UseInMemoryDatabase(databaseName: "RepositoryTest")
                .Options;
        }

        MasterDataContext GetContext()
        {
            return new MasterDataContext(_options);
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
            }

        }
    }
}
