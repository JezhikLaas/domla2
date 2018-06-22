using Beginor.NHibernate.NpgSql;
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
    public class AdministrationUnitsFeatureRepositoryTest : IDisposable
    {
        private readonly string _testFile;

        public AdministrationUnitsFeatureRepositoryTest()
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

            foreach (var mapping in versioned)
            {
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
            if (disposing)
            {
                ConnectionFactory.Shutdown();
                File.Delete(_testFile);
            }
        }

        IDataContext GetContext()
        {
            return new DataContext();
        }

        (Guid, int) InsertAdministrationUnitFeature()
        {
            var unit = AdministrationUnitsFeatureBuilder.New.Build();

            using (var context = GetContext())
            {
                var repository = new BaseSettingsRepository(context);
                repository.Insert(unit);
            }

            return (unit.Id, unit.Version);
        }

        [Fact(DisplayName = "BasicSettingsRepository can insert AdministrationUnitsFeature")]
        public void AdministrationUnitFeatureRepository_can_insert_AdministrationUnitsFeature()
        {
            InsertAdministrationUnitFeature();

            using (var context = GetContext())
            {
                var repository = new BaseSettingsRepository(context);
                var stored = repository.List().ToList();

                Assert.Collection(stored, u => Assert.Equal("Wohnflaeche", u.Title));
            }
        }

        [Fact(DisplayName = "BasicSettingsRepository yields null for unknown id")]
        public void BasicSettingsRepository_yields_null_for_unknown_id()
        {
            InsertAdministrationUnitFeature();

            using (var context = GetContext())
            {
                var repository = new BaseSettingsRepository(context);
                Assert.Null(repository.Load(Guid.NewGuid()));
            }
        }

        [Fact(DisplayName = "BasicSettingsRepository can update AdministrationUnitFeature")]
        public void BasicSettingsRepository_can_update_AdministrationUnitFeature()
        {
            var info = InsertAdministrationUnitFeature();
            var unit = AdministrationUnitsFeatureBuilder.New
                .WithId(info.Item1)
                .WithTitle("Modernisierungsjahr")
                .WithVersion(info.Item2)
                .Build();

            using (var context = GetContext())
            {
                var repository = new BaseSettingsRepository(context);
                repository.Update(unit);
            }
            using (var context = GetContext())
            {
                var repository = new BaseSettingsRepository(context);
                var modified = repository.Load(info.Item1);
                Assert.Equal("Modernisierungsjahr", modified.Title);
            }
        }
    }
}