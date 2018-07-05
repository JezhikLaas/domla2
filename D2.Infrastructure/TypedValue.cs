using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace D2.Infrastructure
{
    [JsonObject(MemberSerialization.Fields)]
    public struct TypedValue : IEquatable<TypedValue>,
        IComparable<TypedValue>,
        IComparable
    {
        private readonly decimal _value;
        private readonly string _unit;
        private readonly int _decimalPlaces;

        public string Unit => _unit;
        public decimal Value => _value;
        public decimal DecimalPlaces => _decimalPlaces;

        [JsonConstructor]
        public TypedValue(decimal value, string unit, int decimalPlaces)
        {
            _value = value;
            _unit = unit;
            _decimalPlaces = decimalPlaces;
        }

        public TypedValue(TypedValue value)
        {
            _value = value._value;
            _unit = value._unit;
            _decimalPlaces = value._decimalPlaces;
        }

        public int CompareTo(TypedValue other)
        {
            if (other._unit != _unit) throw new Exception(string.Format("cannot compare '{0}' to '{1}'", other._unit, _unit));
            return _value.CompareTo(other._value);
        }

        public int CompareTo(object obj)
        {
            if (obj == null || obj.GetType() != typeof(TypedValue)) throw new ArgumentException("object is null or not of type TypedValue");
            return CompareTo((TypedValue)obj);
        }

        public bool Equals(TypedValue other)
        {
            return _unit == other._unit && _value == other._value;
        }

        public static TypedValue operator -(TypedValue value)
        {
            return new TypedValue(-value._value, value._unit, value._decimalPlaces);
        }

        public static TypedValue operator +(TypedValue value)
        {
            return value;
        }

        static InvalidOperationException DifferentUnits()
        {
            return new InvalidOperationException("cannot calculate with differing units");
        }

        public static TypedValue operator +(TypedValue left, TypedValue right)
        {
            if (left._unit != right.Unit)
            {
                throw DifferentUnits();
            }

            return new TypedValue(left._value + right._value, left.Unit, left._decimalPlaces);
        }

        public static TypedValue operator -(TypedValue left, TypedValue right)
        {
            if (left._unit != right.Unit)
            {
                throw DifferentUnits();
            }

            return new TypedValue(left._value - right._value, left.Unit, left._decimalPlaces);
        }

        public static TypedValue operator *(TypedValue left, Decimal right)
        {
            return new TypedValue(left._value * right, left.Unit, left._decimalPlaces);
        }

        public static TypedValue operator /(TypedValue left, Decimal right)
        {
            return new TypedValue(left._value / right, left.Unit, left._decimalPlaces);
        }

        public static bool operator ==(TypedValue left, TypedValue right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypedValue left, TypedValue right)
        {
            return !left.Equals(right);
        }

        public static bool operator >=(TypedValue left, TypedValue right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <=(TypedValue left, TypedValue right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(TypedValue left, TypedValue right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <(TypedValue left, TypedValue right)
        {
            return left.CompareTo(right) < 0;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode() | (_unit != null ? _unit.GetHashCode() : 0);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(TypedValue)) return false;
            if (((TypedValue)obj)._unit != _unit) return false;
            return ((TypedValue)obj)._value == _value;
        }


        public override string ToString()
        {
            return string.Format("{0} {1}", _value, _unit);
        }
    }
}
