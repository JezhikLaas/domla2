using System;
using Xunit;

namespace D2.Infrastructure.Test
{
    public class VariantTest
    {
        [Fact(DisplayName = "Variant from decimal types Variant as decimal")]
        public void VariantFromDecimalYieldsDecimalTyping()
        {
            var target = new Variant(1.0m);
            Assert.Equal(VariantTag.Decimal, target.Tag);
        }
        
        [Fact(DisplayName = "Variant from datetime types Variant as datetime")]
        public void VariantFromDatetimeYieldsDatetimeTyping()
        {
            var target = new Variant(new DateTime(2000, 1, 1));
            Assert.Equal(VariantTag.DateTime, target.Tag);
        }
        
        [Fact(DisplayName = "Variant from string types Variant as string")]
        public void VariantFromStringYieldsStringTyping()
        {
            var target = new Variant("Hello");
            Assert.Equal(VariantTag.String, target.Tag);
        }
    }
}