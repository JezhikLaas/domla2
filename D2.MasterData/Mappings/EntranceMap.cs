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
            Schema("md");
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Nullable();
            Version(x => x.Version)
                .Access.BackingField()
                .Generated.Never()
                .Not.Nullable();
            OptimisticLock.Version();
            References(x => x.AdministrationUnit)
                .Access.BackingField()
                .Cascade.SaveUpdate()
                .Not.Nullable();
            // ReSharper disable once VirtualMemberCallInConstructor
            // member is not overwritten in descendents
            Component(x => x.Address);
            HasMany(x => x.SubUnits)
                .Cascade.AllDeleteOrphan()
                .Cascade.Merge()
                .Inverse();
        }
    }
}