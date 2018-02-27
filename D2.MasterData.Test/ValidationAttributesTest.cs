using D2.MasterData.Infrastructure;
using D2.MasterData.Infrastructure.Validation;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace D2.MasterData.Test
{
    public class ValidationAttributesTest
    {
        IParameterValidator _validator;

        public ValidationAttributesTest()
        {
            _validator = Substitute.For<IParameterValidator>();
        }

        [Fact(DisplayName = "NotNullOrEmptyAttribute detects empty string")]
        public void NotNullOrEmpty_detects_empty_string()
        {
            var target = new NotNullOrEmptyAttribute();
            var check = target.Error(_validator, string.Empty, typeof(string));
            Assert.Equal("must not be empty", check);
        }

        [Fact(DisplayName = "NotNullOrEmptyAttribute detects null string")]
        public void NotNullOrEmpty_detects_null_string()
        {
            var target = new NotNullOrEmptyAttribute();
            var check = target.Error(_validator, null, typeof(string));
            Assert.Equal("must not be null", check);
        }

        [Fact(DisplayName = "NotNullOrEmptyAttribute detects null object")]
        public void NotNullOrEmpty_detects_null_object()
        {
            var target = new NotNullOrEmptyAttribute();
            var check = target.Error(_validator, null, typeof(object));
            Assert.Equal("must not be null", check);
        }

        [Fact(DisplayName = "NotNullOrEmptyAttribute validates non empty string")]
        public void NotNullOrEmpty_validates_non_empty_string ()
        {
            var target = new NotNullOrEmptyAttribute();
            var check = target.Error(_validator, "Foo", typeof(string));
            Assert.Null(check);
        }

        [Fact(DisplayName = "NotNullOrEmptyAttribute validates non null object")]
        public void NotNullOrEmpty_validates_non_null_object()
        {
            var target = new NotNullOrEmptyAttribute();
            var check = target.Error(_validator, new object(), typeof(object));
            Assert.Null(check);
        }

        [Fact(DisplayName = "NotNullOrEmptyAttribute detects empty enumerable")]
        public void NotNullOrEmpty_detects_empty_enumerable()
        {
            var target = new NotNullOrEmptyAttribute();
            var check = target.Error(_validator, new string[] { }, typeof(IEnumerable<string>));
            Assert.Equal("must not be empty", check);
        }
    }
}
