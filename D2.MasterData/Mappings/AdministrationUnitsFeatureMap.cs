using D2.Infrastructure;
using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class AdministrationUnitsFeatureMap : ClassMap<AdministrationUnitsFeature>
    {
        public AdministrationUnitsFeatureMap()
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
            Version(x => x.Version)
                .Access.BackingField()
                .Generated.Never()
                .Not.Nullable();
            OptimisticLock.Version();
        }
    }
}
