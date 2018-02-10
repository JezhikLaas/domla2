using D2.MasterData.Models;
using System;
using Xunit;

namespace D2.MasterData.Test
{
    public class CountryInfoTest
    {
        [Fact]
        public void Iso2_does_not_accept_three_letters()
        {
            var info = new CountryInfo();
            Assert.Throws<ArgumentException>(() => info.Iso2 = "DEU");
        }

        [Fact]
        public void Iso2_does_not_accept_one_letters()
        {
            var info = new CountryInfo();
            Assert.Throws<ArgumentException>(() => info.Iso2 = "D");
        }

        [Fact]
        public void Iso2_does_not_accept_null_letters()
        {
            var info = new CountryInfo();
            Assert.Throws<ArgumentException>(() => info.Iso2 = null);
        }

        [Fact]
        public void Iso2_does_accept_two_letters()
        {
            var info = new CountryInfo();
            info.Iso2 = "DE";
            Assert.Equal("DE", info.Iso2);
        }
    }
}
