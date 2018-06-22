using D2.Infrastructure;
using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class AdministrationUnitPropertyCreateMap: ClassMap<AdministrationUnitProperty>
    {
        public AdministrationUnitPropertyCreateMap()
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
            References(x => x.AdministrationUnit)
                .Access.BackingField()
                .Not.Nullable();
        }
    }

    public class AdministrationUnitPropertyMap: AdministrationUnitPropertyCreateMap
    {
        public AdministrationUnitPropertyMap()
        {
            Version(x => x.Version)
                .Column("xmin")
                .Generated.Always();
        }
    }
}
