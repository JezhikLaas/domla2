using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class SubUnitMap : ClassMap<SubUnit>
    {
        public SubUnitMap()
        {
            Table("subunits");
            Id(x => x.Id);
            Schema("md");
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            Map(x => x.Number)
                .Access.BackingField()
                .Nullable();
            Version(x => x.Version)
                .Access.BackingField()
                .Generated.Never()
                .Nullable();
            OptimisticLock.Version();
            References(x => x.AdministrationUnit)
                .Access.BackingField()
                .Cascade.SaveUpdate()
                .Not.Nullable();
        }
    }
}