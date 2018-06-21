using D2.Infrastructure;
using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class AdministrationUnitsFeatureCreateMap : ClassMap<AdministrationUnitsFeature>
    {
        public AdministrationUnitsFeatureCreateMap()
        {
            Table("administrationunitsfeatures");
            Id(x => x.Id);
            Schema("md");
            Map(x => x.Title)
                .Access.BackingField()
                .Length(256)
                .Not
                .Nullable();
            Map(x => x.Description)
                .Access.BackingField()
                .Length(1024)
                .Nullable();
            Map(x => x.Tag)
                .CustomType<GenericEnumMapper<VariantTag>>()
                .Access.BackingField()
                .Length(32)
                .Not
                .Nullable();
            Map(x => x.TypedValueDecimalPlace)
                .Access.BackingField()
                .Nullable();
            Map(x => x.TypedValueUnit)
                .Access.BackingField()
                .Length(256)
                .Nullable();
        }
    }

    public class AdministrationUnitsFeatureMap : AdministrationUnitsFeatureCreateMap
    {
        public AdministrationUnitsFeatureMap()
        {
            Version(x => x.Version)
                .Column("xmin")
                .Generated.Always();
        }
    }
}
