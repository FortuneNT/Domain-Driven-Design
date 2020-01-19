using Newtonsoft.Json;
using System;
using System.Reflection;

namespace DDD.Types.Serializers.Json
{
    public class JsonValueTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();
            var property = type.GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance);
            var val = property.GetValue(value);

            writer.WriteValue(val);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type argumentType = GetValueTypeArgumentType(objectType);
            object value = serializer.Deserialize(reader, argumentType);
            object valueType = Activator.CreateInstance(objectType, value);
            return valueType;
        }

        public override bool CanConvert(Type objectType) => TypeImplementsGenericValueType(objectType);

        private static bool TypeImplementsGenericValueType(Type type)
            => type != null
            && type.BaseType != null
            && (TypeIsGenericValueType(type.BaseType) || TypeImplementsGenericValueType(type.BaseType));

        private static Type GetValueTypeArgumentType(Type objectType)
            => TypeIsGenericValueType(objectType)
             ? objectType.GetGenericArguments()[0]
             : GetValueTypeArgumentType(objectType.BaseType);

        private static bool TypeIsGenericValueType(Type type)
            => type.IsGenericType
            && type.GetGenericTypeDefinition() == typeof(ValueType<>);
    }
}