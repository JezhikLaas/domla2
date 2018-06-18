using System;
using System.Globalization;

namespace D2.Infrastructure
{
    public class Variant
    {
        public VariantTag Tag { get; }

        public decimal Decimal => AsDecimal();

        public DateTime DateTime => AsDateTime();

        public string String => AsString();
        
        public Variant()
        { }

        public Variant(decimal value)
        {
            Tag = VariantTag.Decimal;
            _storage = value;
        }

        public Variant(DateTime value)
        {
            Tag = VariantTag.DateTime;
            _storage = value;
        }

        public Variant(string value)
        {
            Tag = VariantTag.String;
            _storage = value;
        }

        private object _storage;

        private decimal AsDecimal()
        {
            switch (Tag) {
                case VariantTag.DateTime:
                    // oder: RaiseInvalidCast(VariantTag.Decimal) ???
                    return ((DateTime) _storage).Ticks;
                case VariantTag.String:
                    throw RaiseInvalidCast(VariantTag.Decimal);
                default:
                    return (decimal) _storage;
            }
        }

        private string AsString()
        {
            switch (Tag) {
                case VariantTag.DateTime:
                    return ((DateTime)_storage).ToString(CultureInfo.CurrentCulture);
                case VariantTag.String:
                    return (string) _storage;
                default:
                    return ((decimal)_storage).ToString(CultureInfo.CurrentCulture);
            }
        }

        private DateTime AsDateTime()
        {
            switch (Tag) {
                case VariantTag.DateTime:
                    return (DateTime) _storage;
                case VariantTag.String:
                    throw RaiseInvalidCast(VariantTag.DateTime);
                default:
                    throw RaiseInvalidCast(VariantTag.DateTime);
            }
        }

        private InvalidCastException RaiseInvalidCast(VariantTag requested)
        {
            return new InvalidCastException($"Requested value as {requested}, but it is {Tag}");
        }
    }

    public enum VariantTag
    {
        None,
        DateTime,
        Decimal,
        String
    }
}