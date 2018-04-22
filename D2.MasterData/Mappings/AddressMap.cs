using D2.MasterData.Models;
using FluentNHibernate.Mapping;

namespace D2.MasterData.Mappings
{
    public class AddressMap : ComponentMap<Address>
    {
        public AddressMap()
        {
            Map(x => x.City)
                .Access.BackingField()
                .Length(100);
            Map(x => x.Number)
                .Access.BackingField()
                .Length(10);
            Map(x => x.PostalCode)
                .Access.BackingField()
                .Length(20);
            Map(x => x.Street)
                .Access.BackingField()
                .Length(150);
            Component(c => c.Country, b =>
            {
                b.Map(x => x.Iso2)
                    .Access.BackingField()
                    .Length(3);
                b.Map(x => x.Iso3)
                    .Access.BackingField()
                    .Length(4);
                b.Map(x => x.Name)
                    .Access.BackingField()
                    .Length(100);
            });
        }
    }
}