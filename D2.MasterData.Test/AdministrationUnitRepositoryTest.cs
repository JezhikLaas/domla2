using D2.MasterData.Repositories.Implementation;
using D2.MasterData.Infrastructure;
using System;
using System.IO;
using System.Linq;
using D2.MasterData.Mappings;
using Xunit;
using D2.MasterData.Test.Helper;
using FluentNHibernate.Cfg;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;

namespace D2.MasterData.Test
{
    public class AdministrationUnitRepositoryTest : IDisposable
    {
        private readonly string _testFile;

        public AdministrationUnitRepositoryTest()
        {
            _testFile = Path.GetTempFileName();
            var configuration = Fluently.Configure()
                .Database(SqliteConfiguration.Standard.UsingFile(_testFile))
                .ExposeConfiguration(BuildSchema);
            ConnectionFactory.Initialize(configuration);
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
                ConnectionFactory.Shutdown();
                File.Delete(_testFile);
            }
        }

        IDataContext GetContext()
        {
            return new DataContext();
        }

        (Guid, int) InsertAdministrationUnit()
        {
            var unit = AdministrationUnitBuilder.New.Build();

            using (var context = GetContext()) {
                var repository = new AdministrationUnitRepository(context);
                repository.Insert(unit);
            }

            return (unit.Id, unit.Version);
        }

        [Fact(DisplayName = "AdministrationUnitRepository can insert AdministrationUnit")]
        public void AdministrationUnitRepository_can_insert_AdministrationUnit()
        {
            InsertAdministrationUnit();

            using (var context = GetContext()) {
                var repository = new AdministrationUnitRepository(context);
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
                var repository = new AdministrationUnitRepository(context);
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
                var repository = new AdministrationUnitRepository(context);
                repository.Update(unit);
            }
            using (var context = GetContext())
            {
                var repository = new AdministrationUnitRepository(context);
                var modified = repository.Load(info.Item1);
                Assert.Equal("Drachenhöhle", modified.Title);
            }
        }
    }
}