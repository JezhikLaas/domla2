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
    public class PostalCodeInfoRepositoryTest : IDisposable
    {
        private readonly string _testFile;

        public PostalCodeInfoRepositoryTest()
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

        (Guid, int) InsertPostalCodeInfo()
        {
            var unit = PostalCodeInfoBuilder.New.Build();

            using (var context = GetContext())
            {
                var repository = new PostalCodeInfoRepository(context);
                repository.Insert(unit);
            }

            return (unit.Id, unit.Version);
        }

        [Fact(DisplayName = "PostalCodeInfoRepository can insert PostalCodeInfo")]
        public void PostalCodeInfoRepository_can_insert_PostalCodeInfo()
        {
            InsertPostalCodeInfo();

            using (var context = GetContext())
            {
                var repository = new PostalCodeInfoRepository(context);
                var stored = repository.List().ToList();

                Assert.Collection(stored, u => Assert.Equal("32602", u.PostalCode));
            }
        }

        [Fact(DisplayName = "PostalCodeInfoRepository yields null for unknown id")]
        public void PostalCodeInfoRepository_yields_null_for_unknown_id()
        {
            InsertPostalCodeInfo();

            using (var context = GetContext())
            {
                var repository = new PostalCodeInfoRepository(context);
                Assert.Null(repository.Load(Guid.NewGuid()));
            }
        }

    }
}
