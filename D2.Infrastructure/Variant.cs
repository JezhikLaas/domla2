using System;
using System.Globalization;

namespace D2.Infrastructure
{
    public class Variant
    {
        public VariantTag Tag { get; }

        public TypedValue Number => AsTypedValue();

        public DateTime DateTime => AsDateTime();

        public string String => AsString();
        
        public Variant()
        { }

        public Variant(TypedValue value)
        {
            Tag = VariantTag.TypedValue;
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

        private TypedValue AsTypedValue()
        {
            switch (Tag) {
                case VariantTag.DateTime:
                    throw RaiseInvalidCast(VariantTag.TypedValue);
                case VariantTag.String:
                    throw RaiseInvalidCast(VariantTag.TypedValue);
                default:
                    return (TypedValue) _storage;
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
                    return ((TypedValue)_storage).ToString();
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
        TypedValue,
        String
    }
}