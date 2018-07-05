using D2.Infrastructure;
using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class AdministrationUnitPropertyMap: ClassMap<AdministrationUnitProperty>
    {
        public AdministrationUnitPropertyMap()
        {
            Table("administrationunitproperty");
            Id(x => x.Id);
            Schema("md");
            Map(x => x.Edit)
                .Access.BackingField()
                .Generated.Always();
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Not.Nullable();
            Map(x => x.Description)
                .Access.BackingField()
                .Length(1024)
                .Nullable();
            Map(x => x.Value)
                .Access.BackingField()
                .CustomType<VariantType>()
                .Not.Nullable();
            Version(x => x.Version)
                .Access.BackingField()
                .Generated.Never()
                .Not.Nullable();
            OptimisticLock.Version();
            References(x => x.AdministrationUnit)
                .Access.BackingField()
                .Cascade.SaveUpdate()
                .Not.Nullable();
        }
    }
}
