﻿using D2.MasterData.Repositories.Implementation;
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
    public class AdministrationUnitFeatureRepositoryTest : IDisposable
    {
        private readonly string _testFile;

        public AdministrationUnitFeatureRepositoryTest()
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
            var unit = AdministrationUnitFeatureBuilder.New.Build();

            using (var context = GetContext())
            {
                var repository = new BasicSettingsRepository(context);
                repository.Insert(unit);
            }

            return (unit.Id, unit.Version);
        }

        [Fact(DisplayName = "BasicSettingsRepository can insert AdministrationUnitFeature")]
        public void AdministrationUnitFeatureRepository_can_insert_AdministrationUnitFeature()
        {
            InsertAdministrationUnitFeature();

            using (var context = GetContext())
            {
                var repository = new BasicSettingsRepository(context);
                var stored = repository.List().ToList();

                Assert.Collection(stored, u => Assert.Equal("Wohnflaeche", u.Title));
            }
        }

        [Fact(DisplayName = "BasicSettingsRepository yields null for unknown id")]
        public void AdministrationUnitFeatureRepository_yields_null_for_unknown_id()
        {
            InsertAdministrationUnitFeature();

            using (var context = GetContext())
            {
                var repository = new BasicSettingsRepository(context);
                Assert.Null(repository.Load(Guid.NewGuid()));
            }
        }

        [Fact(DisplayName = "BasicSettingsRepository can update AdministrationUnitFeature")]
        public void BasicSettingsRepository_can_update_AdministrationUnitFeature()
        {
            var info = InsertAdministrationUnitFeature();
            var unit = AdministrationUnitFeatureBuilder.New
                .WithId(info.Item1)
                .WithTitle("Modernisierungsjahr")
                .WithVersion(info.Item2)
                .Build();

            using (var context = GetContext())
            {
                var repository = new BasicSettingsRepository(context);
                repository.Update(unit);
            }
            using (var context = GetContext())
            {
                var repository = new BasicSettingsRepository(context);
                var modified = repository.Load(info.Item1);
                Assert.Equal("Modernisierungsjahr", modified.Title);
            }
        }
    }
}