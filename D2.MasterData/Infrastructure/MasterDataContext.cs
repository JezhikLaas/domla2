using D2.Common;
using D2.MasterData.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace D2.MasterData.Infrastructure
{
    public class MasterDataContext : DbContext
    {
        public MasterDataContext()
        { }

        public MasterDataContext(DbContextOptions<MasterDataContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false) {
                var builder = new NpgsqlConnectionStringBuilder();
                var options = ServiceConfiguration.connectionInfo;

                builder.ApplicationName = options.Identifier;
                builder.Database = options.Name;
                builder.Host = options.Host;
                builder.Password = options.Password;
                builder.Username = options.User;
                builder.Port = options.Port;

                optionsBuilder.UseNpgsql(builder.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<AdministrationUnit>()
                .OwnsOne(
                    unit => unit.Address,
                    builder => builder.OwnsOne(address => address.Country)
            );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AdministrationUnit> AdministrationUnits { get; set; }
    }
}
