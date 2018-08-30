using D2.Infrastructure;
using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class AdministrationUnitMap : ClassMap<AdministrationUnit>
    {
        public AdministrationUnitMap()
        {
            Table("administrationunits");
            Id(x => x.Id);
            Schema("md");
            Map(x => x.UserKey)
                .Access.BackingField()
                .Length(256)
                .Not
                .Nullable();
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            Map(x => x.YearOfConstruction)
                .Access.BackingField()
                .CustomType<YearMonthType>()
                .Nullable();
            Version(x => x.Version)
                .Access.BackingField()
                .Generated.Never()
                .Not.Nullable();
            OptimisticLock.Version();
            HasMany(x => x.Entrances)
                .Cascade.AllDeleteOrphan()
                .Cascade.Merge()
                .Inverse();
            HasMany(x => x.AdministrationUnitProperties)
                .Cascade.AllDeleteOrphan()
                .Cascade.Merge()
                .Inverse();
            HasMany(x => x.UnboundSubUnits)
                .Cascade.AllDeleteOrphan()
                .Cascade.Merge()
                .Inverse();
        }
    }
}