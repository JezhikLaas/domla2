using System;
using Xunit;

namespace D2.Infrastructure.Test
{
    public class VariantTest
    {
        [Fact(DisplayName = "Variant from decimal types Variant as decimla")]
        public void TypedValueOperatorPlus()
        {
            var target = new Variant(1.0m);
            Assert.Equal(VariantTag.Decimal, target.Tag);
        }
    }
}