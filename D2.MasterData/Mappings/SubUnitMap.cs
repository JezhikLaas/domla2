using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class SubUnitCreateMap : ClassMap<SubUnit>
    {
        public SubUnitCreateMap()
        {
            Table("subunits");
            Id(x => x.Id);
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            Map(x => x.Floor)
                .Access.BackingField()
                .Nullable();
            Map(x => x.Number)
                .Access.BackingField()
                .Not.Nullable();
            Map(x => x.Usage)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            References(x => x.Entrance)
                .Access.BackingField();
        }
    }

    public class SubUnitMap : SubUnitCreateMap
    {
        public SubUnitMap()
        {
            Version(x => x.Version)
                .Column("xmin")
                .Generated.Always();
        }
    }
}