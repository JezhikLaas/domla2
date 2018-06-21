using System;
using D2.MasterData.Infrastructure;
using D2.MasterData.Infrastructure.Validation;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace D2.MasterData.Test
{
    public class ValidationAttributesTest
    {
        readonly IParameterValidator _validator;

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

        [Fact(DisplayName = "MaxLengthAttribute detects text thats too long")]
        public void MaxLength_detects_text_thats_too_long()
        {
            var target = new MaxLengthAttribute(5);
            var check = target.Error(_validator, "123456", typeof(string));
            Assert.Equal("text value contains too much characters", check);
        }

        [Fact(DisplayName = "MaxLengthAttribute ignores fitting text")]
        public void MaxLength_ignores_fitting_text()
        {
            var target = new MaxLengthAttribute(5);
            var check = target.Error(_validator, "12345", typeof(string));
            Assert.Null(check);
        }

        [Fact(DisplayName = "MaxLengthAttribute detects containers with too much elements")]
        public void MaxLength_detects_containers_with_too_much_elements()
        {
            var target = new MaxLengthAttribute(2);
            var check = target.Error(_validator, new[]{ "", "", ""}, typeof(IEnumerable<string>));
            Assert.Equal("container contains too much elements", check);
        }

        [Fact(DisplayName = "MaxLengthAttribute ignores fitting containers")]
        public void MaxLength_ignores_fitting_containers()
        {
            var target = new MaxLengthAttribute(2);
            var check = target.Error(_validator, new[]{ 0, 0}, typeof(IEnumerable<int>));
            Assert.Null(check);
        }

        [Fact(DisplayName = "RangeAttribute detects ints which are too large")]
        public void Range_detects_ints_which_are_too_large()
        {
            var target = new RangeAttribute {MaxValue = 5};
            var check = target.Error(_validator, 6, typeof(int));
            Assert.Equal("Value is greater then MaxValue", check);
        }

        [Fact(DisplayName = "RangeAttribute detects nullable ints which are too large")]
        public void Range_detects_nullable_ints_which_are_too_large()
        {
            var target = new RangeAttribute {MaxValue = 5};
            var check = target.Error(_validator, new Nullable<int>(6), typeof(int?));
            Assert.Equal("Value is greater then MaxValue", check);
        }

        [Fact(DisplayName = "RangeAttribute let ints pass that fit")]
        public void Range_let_ints_pass_then_fit()
        {
            var target = new RangeAttribute {MaxValue = 5};
            var check = target.Error(_validator, 4, typeof(int));
            Assert.Null(check);
        }

        [Fact(DisplayName = "RangeAttribute let nullable ints pass that fit")]
        public void Range_let_nullable_ints_pass_that_fit()
        {
            var target = new RangeAttribute {MaxValue = 5};
            var check = target.Error(_validator, new Nullable<int>(4), typeof(int?));
            Assert.Null(check);
        }

        [Fact(DisplayName = "RangeAttribute detects null when MinValue is set")]
        public void Range_detects_null_when_MinValue_is_set()
        {
            var target = new RangeAttribute {MinValue = 5};
            var check = target.Error(_validator, null, typeof(int?));
            Assert.Equal("Value is null", check);
        }
    }
}
