using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class EntranceMap : ClassMap<Entrance>
    {
        public EntranceMap()
        {
            Table("entrances");
            Id(x => x.Id);
            Version(x => x.Version)
                .Column("xmin")
                .Generated.Always();
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            References(x => x.AdministrationUnit)
                .Access.BackingField()
                .Not.Nullable();
            Component(x => x.Address);
            HasMany(x => x.SubUnits)
                .Cascade.AllDeleteOrphan()
                .Inverse();
        }
    }
}