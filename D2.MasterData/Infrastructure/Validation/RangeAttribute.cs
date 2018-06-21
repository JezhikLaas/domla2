using System;
using Ice;

namespace D2.MasterData.Infrastructure.Validation
{
    public class RangeAttribute : ParameterValidationAttribute
    {
        public decimal MinValue { get; set; }
        
        public decimal MaxValue { get; set; }

        public RangeAttribute()
        {
            MinValue = decimal.MinValue;
            MaxValue = decimal.MaxValue;
        }
        
        public override string Error(IParameterValidator validator, object value, Type propertyType)
        {
            if (propertyType == typeof(decimal)) {
                return CheckRange((decimal)value);
            }
            if (propertyType == typeof(decimal?)) {
                return CheckRangeForNullable((decimal?)value);
            }
            if (propertyType == typeof(int)) {
                return CheckRange((int)value);
            }
            if (propertyType == typeof(int?)) {
                return CheckRangeForNullable((int?)value);
            }
            if (propertyType == typeof(float)) {
                return CheckRange((decimal)(float)value);
            }
            if (propertyType == typeof(float?)) {
                return CheckRangeForNullable((float?)value);
            }
            if (propertyType == typeof(double)) {
                return CheckRange((decimal)(double)value);
            }
            if (propertyType == typeof(double?)) {
                return CheckRangeForNullable((double?)value);
            }

            return null;
        }

        string CheckRange(decimal value)
        {
            if (value > MaxValue) return "Value is greater then MaxValue";
            if (value < MinValue) return "Value is less then MinValue";
            return null;
        }

        string CheckRange<T>(T value)
        {
            if (Convert.ToDecimal(value) > MaxValue) return "Value is greater then MaxValue";
            if (Convert.ToDecimal(value) < MinValue) return "Value is less then MinValue";
            return null;
        }

        string CheckRangeForNullable<T>(T? value) where T: struct
        {
            if (value == null && MinValue != decimal.MinValue) return "Value is null";
            if (Convert.ToDecimal(value.Value) > MaxValue) return "Value is greater then MaxValue";
            if (Convert.ToDecimal(value.Value) < MinValue) return "Value is less then MinValue";
            return null;
        }
    }
}
