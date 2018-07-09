using System;
using Newtonsoft.Json;
using Xunit;

namespace D2.Infrastructure.Test
{
    public class VariantTest
    {
        [Fact(DisplayName = "Variant from TypedValue types Variant as TypedValue")]
        public void VariantFromTypedValueYieldsTypedValueTyping()
        {
            var target = new Variant(new TypedValue(1,"meter", 2));
            Assert.Equal(VariantTag.TypedValue, target.Tag);
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


        [Fact(DisplayName = "Variant get Number Value from TypedValue")]
        public void VariantGetNumberValueFromTypedValue()
        {
            var target = new Variant(new TypedValue(1, "meter", 2));
            var result = target.Number.Value;
            Assert.Equal(1,result);
        }

        [Fact(DisplayName = "Variant get Number from String throws Exception")]
        public void VariantGetNumberFromStringThrowsException()
        {
            var target = new Variant(new String("1"));
            Assert.Throws<InvalidCastException>(()=> target.Number.Value);
        }

        [Fact(DisplayName = "Variant get Number from DateTime throws Exception")]
        public void VariantGetNumbeFromDateTimeThrowsException()
        {
            var target = new Variant(new DateTime(2018, 6, 19));
            Assert.Throws<InvalidCastException>(() => target.Number.Value);
        }

        [Fact(DisplayName = "Variant get DateTime Value from DateTime")]
        public void VariantGetDateTimeValueFromDateTime()
        {
            var target = new Variant(DateTime.Now);
            Assert.Equal(DateTime.Now.ToString(), target.DateTime.ToString());
        }

        [Fact(DisplayName = "Variant get DateTime from TypedValue throws Exception")]
        public void VariantGetDateTimeFromNumberThrowsException()
        {
            var target = new Variant(new TypedValue(1, "meter", 2));
            Assert.Throws<InvalidCastException>(() => target.DateTime.ToString()); 
        }

        [Fact(DisplayName = "Variant get DateTime from String throws Exception")]
        public void VariantGetDateTimeFromStringThrowsException()
        {
            var target = new Variant(new String("1"));
            Assert.Throws<InvalidCastException>(() => target.DateTime.ToString());
        }

        [Fact(DisplayName = "Variant get String Value from String")]
        public void VariantGetStringValueFromDateTime()
        {
            var target = new Variant(new String("1"));
            Assert.Equal("1", target.String.ToString());
        }

        [Fact(DisplayName = "Variant get String from TypedValue")]
        public void VariantGetStringValueFromTypedValue()
        {
            var target = new Variant(new TypedValue(1, "meter", 2));
            Assert.Equal("1 meter", target.String);
        }

        [Fact(DisplayName = "Variant get String from DateTime does not throw Exception")]
        public void VariantGetDateTimeFromDateTimeDoesNotThrowException()
        {
            var target = new Variant(DateTime.Now);
            Assert.Equal(DateTime.Now.ToString(), target.String);
        }

        [Fact(DisplayName = "Variant can be set to DateTime typed null")]
        public void VariantCanBeSetToDateTimeNull()
        {
            var target = new Variant(VariantTag.DateTime);
            Assert.Equal(default(DateTime), target.DateTime);
        }

        [Fact(DisplayName = "Variant can be set to TypedValue typed null")]
        public void VariantCanBeSetToTypedValueNull()
        {
            var target = new Variant(VariantTag.TypedValue);
            Assert.Equal(default(TypedValue), target.Number);
        }

        [Fact(DisplayName = "Variant can be set to String typed null")]
        public void VariantCanBeSetToStringNull()
        {
            var target = new Variant(VariantTag.String);
            Assert.Equal(default(string), target.String);
        }

        [Fact(DisplayName = "Null String Variant can be serialized and deserialized")]
        public void SerializeAndDeserializeNullStringVariant()
        {
            var target = new Variant(VariantTag.String);
            var serialized = JsonConvert.SerializeObject(target);
            var restored = JsonConvert.DeserializeObject<Variant>(serialized);
            
            Assert.NotNull(restored);
            Assert.True(restored.IsNull);
            Assert.Equal(VariantTag.String, restored.Tag);
        }

        [Fact(DisplayName = "Null DateTime Variant can be serialized and deserialized")]
        public void SerializeAndDeserializeNullDateTimeVariant()
        {
            var target = new Variant(VariantTag.DateTime);
            var serialized = JsonConvert.SerializeObject(target);
            var restored = JsonConvert.DeserializeObject<Variant>(serialized);
            
            Assert.NotNull(restored);
            Assert.True(restored.IsNull);
            Assert.Equal(VariantTag.DateTime, restored.Tag);
        }

        [Fact(DisplayName = "Null TypedValue Variant can be serialized and deserialized")]
        public void SerializeAndDeserializeNullTypedValueVariant()
        {
            var target = new Variant(VariantTag.TypedValue);
            var serialized = JsonConvert.SerializeObject(target);
            var restored = JsonConvert.DeserializeObject<Variant>(serialized);
            
            Assert.NotNull(restored);
            Assert.True(restored.IsNull);
            Assert.Equal(VariantTag.TypedValue, restored.Tag);
        }
    }
}