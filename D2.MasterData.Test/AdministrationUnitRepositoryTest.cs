using Beginor.NHibernate.NpgSql;
using D2.MasterData.Repositories.Implementation;
using D2.MasterData.Infrastructure;
using System;
using System.IO;
using System.Linq;
using D2.MasterData.Mappings;
using Xunit;
using D2.MasterData.Test.Helper;
using D2.Service.Controller;
using FluentNHibernate.Cfg;
using Microsoft.Extensions.Logging;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;
using NSubstitute;

namespace D2.MasterData.Test
{
    public class AdministrationUnitRepositoryTest : IDisposable
    {
        private readonly string _testFile;

        private readonly IConnectionFactory _connectionFactory;

        public AdministrationUnitRepositoryTest()
        {
            var logger = Substitute.For<ILogger>();
            _connectionFactory = new ConnectionFactory(logger);
            _testFile = Path.GetTempFileName();
            var configuration = Fluently.Configure()
                .Database(SqliteConfiguration.Standard.UsingFile(_testFile))
                .ExposeConfiguration(BuildSchema);
            _connectionFactory.Initialize(configuration);
        }
        
        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            var versioned = from mapping in config.ClassMappings
                            where mapping.Version != null
                            select mapping;
            
            foreach (var mapping in versioned) {
                mapping.Version.Generation = PropertyGeneration.Never;
                mapping.Version.IsInsertable = true;
                mapping.Version.IsUpdateable = true;
            }

            new SchemaExport(config).Create(false, true);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing) {
                _connectionFactory.Shutdown();
                File.Delete(_testFile);
            }
        }

        IDataContext GetContext()
        {
            var callContext = Substitute.For<ICallContext>();
            callContext["dbkey"].Returns((string)null);
            
            return new DataContext(callContext, _connectionFactory);
        }

        (Guid, int) InsertAdministrationUnit()
        {
            var unit = AdministrationUnitBuilder.New.Build();

            using (var context = GetContext()) {
                var repository = new AdministrationUnitsRepository(context);
                repository.Insert(unit);
            }

            return (unit.Id, unit.Version);
        }

        [Fact(DisplayName = "AdministrationUnitRepository can insert AdministrationUnit")]
        public void AdministrationUnitRepository_can_insert_AdministrationUnit()
        {
            InsertAdministrationUnit();

            using (var context = GetContext()) {
                var repository = new AdministrationUnitsRepository(context);
                var stored = repository.List().ToList();

                Assert.Collection(stored, u => Assert.Equal("03", u.UserKey));
                Assert.Collection(stored,
                    u => Assert.Collection(u.Entrances, e => Assert.Equal("Eingang 49", e.Title)));
            }
        }

        [Fact(DisplayName = "AdministrationUnitRepository yields null for unknown id")]
        public void AdministrationUnitRepository_yields_null_for_unknown_id()
        {
            InsertAdministrationUnit();

            using (var context = GetContext()) {
                var repository = new AdministrationUnitsRepository(context);
                Assert.Null(repository.Load(Guid.NewGuid()));
            }
        }

        [Fact(DisplayName = "AdministrationUnitRepository can update AdministrationUnit")]
        public void AdministrationUnitRepository_can_update_AdministrationUnit()
        {
            var info = InsertAdministrationUnit();
            var unit = AdministrationUnitBuilder.New
                .WithId(info.Item1)
                .WithTitle("Drachenhöhle")
                .WithVersion(info.Item2)
                .Build();

            using (var context = GetContext())
            {
                var repository = new AdministrationUnitsRepository(context);
                repository.Update(unit);
            }
            using (var context = GetContext())
            {
                var repository = new AdministrationUnitsRepository(context);
                var modified = repository.Load(info.Item1);
                Assert.Equal("Drachenhöhle", modified.Title);
            }
        }
    }
}