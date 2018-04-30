using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class EntranceCreateMap : ClassMap<Entrance>
    {
        public EntranceCreateMap()
        {
            Table("entrances");
            Id(x => x.Id);
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
            // ReSharper disable once VirtualMemberCallInConstructor
            // member is not overwritten in descendents
            Component(x => x.Address);
            HasMany(x => x.SubUnits)
                .Cascade.AllDeleteOrphan()
                .Inverse();
        }
    }

    public class EntranceMap : EntranceCreateMap
    {
        public EntranceMap()
        {
            Version(x => x.Version)
                .Column("xmin")
                .Generated.Always();
        }
    }
}