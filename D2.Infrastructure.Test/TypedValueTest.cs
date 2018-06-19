using System;
using Xunit;

namespace D2.Infrastructure.Test
{
    public class TypedValueTest
    {
        [Fact(DisplayName = "TypedValue Operator Plus")]
        public void TypedValueOperatorPlus()
        {
            TypedValue meter = new TypedValue(10, "Meter", 2);
            TypedValue room = new TypedValue(3, "Zimmer", 2);
            Assert.Throws<InvalidOperationException>(() => meter + room);
        }

        [Fact(DisplayName = "TypedValue Operator Minus")]
        public void TypedValueOperatorMinus()
        {
            TypedValue meter = new TypedValue(10, "Meter", 2);
            TypedValue room = new TypedValue(3, "Zimmer", 2);
            Assert.Throws<InvalidOperationException>(() => meter - room);
        }

        [Fact(DisplayName = "TypedValue Operator Multiply")]
        public void TypedValueOperatorMultiply()
        {
            TypedValue meter = new TypedValue(10, "Meter", 2);
            decimal operand = 20;
            var result = meter * operand;
            Assert.Equal(200, result.Value, 2);
        }

        [Fact(DisplayName = "TypedValue Operator Divide")]
        public void TypedValueOperatorDivide()
        {
            TypedValue meter = new TypedValue(10, "Meter", 2);
            decimal operand = 2;
            var result = meter / operand;
            Assert.Equal(5, result.Value, 2);
        }

        [Fact(DisplayName = "TypedValue Operator Greater")]
        public void TypedValueOperatorGreater()
        {
            TypedValue meter = new TypedValue(10, "Meter", 2);
            TypedValue room = new TypedValue(3, "Zimmer", 2);
            Assert.Throws<Exception>(() => meter > room);
        }

        [Fact(DisplayName = "TypedValue Operator Lesser")]
        public void TypedValueOperatorLesser()
        {
            TypedValue meter = new TypedValue(3, "Meter", 2);
            TypedValue room = new TypedValue(10, "Zimmer", 2);
            Assert.Throws<Exception>(() => meter < room);
        }
        
        [Fact(DisplayName = "TypedValue Operator Equal False")]
        public void TypedValueOperatorEqualFalse()
        {
            TypedValue meter = new TypedValue(3, "Meter", 2);
            TypedValue room = new TypedValue(3, "Zimmer", 2);
            var result = meter == room;
            Assert.False(result);
        }


        [Fact(DisplayName = "TypedValue Operator Equal True")]
        public void TypedValueOperatorEqualTrue()
        {
            TypedValue meter = new TypedValue(3, "Meter", 2);
            TypedValue meter2 = new TypedValue(3, "Meter", 2);
            var result = meter == meter2;
            Assert.True(result);
        }

        [Fact(DisplayName = "TypedValue Operator Greater Equal Throw")]
        public void TypedValueOperatorGreaterEqualThrow()
        {
            TypedValue meter = new TypedValue(3, "Meter", 2);
            TypedValue room = new TypedValue(3, "Room", 2);
            Assert.Throws<Exception>(() => meter >= room);
        }

        [Fact(DisplayName = "TypedValue Operator Greater Equal True")]
        public void TypedValueOperatorGreaterEqualTrue()
        {
            TypedValue meter = new TypedValue(3, "Meter", 2);
            TypedValue meter2 = new TypedValue(3, "Meter", 2);
            var result = meter >= meter2;
            Assert.True(result);
        }

        [Fact(DisplayName = "TypedValue Operator Lesser Equal Throw")]
        public void TypedValueOperatorLesserEqualThrow()
        {
            TypedValue meter = new TypedValue(3, "Meter", 2);
            TypedValue room = new TypedValue(3, "Room", 2);
            Assert.Throws<Exception>(() => meter <= room);
        }

        [Fact(DisplayName = "TypedValue Operator Greater Equal True")]
        public void TypedValueOperatorGreaterLesserTrue()
        {
            TypedValue meter = new TypedValue(3, "Meter", 2);
            TypedValue meter2 = new TypedValue(3, "Meter", 2);
            var result = meter <= meter2;
            Assert.True(result);
        }

        [Fact(DisplayName = "TypedValue Operator Reverse Plus")]
        public void TypedValueOperatorReversePlus()
        {
            TypedValue meter = new TypedValue(-3, "Meter", 2);
            var result = +meter;
            Assert.Equal(-3,result.Value);
        }

        [Fact(DisplayName = "TypedValue Operator Reverse Minus")]
        public void TypedValueOperatorReverseMinus()
        {
            TypedValue meter = new TypedValue(3, "Meter", 2);
            var result = -meter;
            Assert.Equal(-3, result.Value);
        }

    }
}