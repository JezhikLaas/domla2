using D2.Common;
using D2.MasterData.Models;
using D2.Service.IoC;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace D2.MasterData.Infrastructure
{
    [RequestScope]
    public class MasterDataContext : DbContext
    {
        public MasterDataContext()
        { }

        public MasterDataContext(DbContextOptions options)
            : base(options)
        { }

        public MasterDataContext(DbContextOptions<MasterDataContext> options)
            : base(options)
        { }

        public DbSet<AdministrationUnit> AdministrationUnits { get; set; }
        public DbSet<Entrance> Entrances { get; set; }
        public DbSet<SubUnit> SubUnits { get; set; }

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

        protected virtual void OnCommonModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<AdministrationUnit>()
                .Property(unit => unit.Edit)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder
                .Entity<Entrance>()
                .OwnsOne(entrance => entrance.Address)
                    .OwnsOne(address => address.Country);

            modelBuilder
                .Entity<Entrance>()
                .Property(unit => unit.Edit)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder
                .Entity<SubUnit>()
                .Property(unit => unit.Edit)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnCommonModelCreating(modelBuilder);

            modelBuilder
                .Entity<AdministrationUnit>()
                .Property(item => item.Version)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnName("xmin")
                .HasColumnType("xid");

            modelBuilder
                .Entity<Entrance>()
                .Property(item => item.Version)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnName("xmin")
                .HasColumnType("xid");

            modelBuilder
                .Entity<SubUnit>()
                .Property(item => item.Version)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnName("xmin")
                .HasColumnType("xid");
        }
    }
}
