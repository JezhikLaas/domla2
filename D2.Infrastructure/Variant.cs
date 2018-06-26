using Newtonsoft.Json;
using System;
using System.Globalization;

namespace D2.Infrastructure
{
    public class Variant
    {
        public VariantTag Tag { get; set; }

        [JsonIgnore]
        public TypedValue Number => AsTypedValue();

        [JsonIgnore]
        public DateTime DateTime => AsDateTime();

        [JsonIgnore]
        public string String => AsString();

        public object Raw
        {
            get => _storage;
            set => _storage = value;
        }
        
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

        public Variant(Variant value)
        {
            Tag = value.Tag;
            _storage = value._storage;
        }

        public Variant(string tag, string value)
        {
            Tag = Enum.Parse<VariantTag>(tag);
            switch (Tag) {
                case VariantTag.DateTime:
                    _storage = new DateTime(long.Parse(value));
                    break;
                case VariantTag.String:
                    _storage = value;
                    break;
                case VariantTag.TypedValue:
                    var elements = value.Split(':');
                    _storage = new TypedValue(decimal.Parse(elements[0]), elements[1], int.Parse(elements[2]));
                    break;
                default:
                    break;
            }
        }
        
        private object _storage;

        public string Encode()
        {
            switch (Tag) {
                case VariantTag.None: return string.Empty;
                case VariantTag.DateTime: return DateTime.Ticks.ToString();
                case VariantTag.String: return String;
                case VariantTag.TypedValue: return $"{Number.Value}:{Number.Unit}:{Number.DecimalPlaces}";
                default: throw new Exception("something went wrong here");
            }
        }
        
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