using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DDD.Types.Serializers.Json.Tests
{
    [TestClass]
    public class JsonValueTypeConverterTests
    {
        private JsonValueTypeConverter _sut;
        private JsonSerializerSettings _serializerSettings;

        private class TestValueTypeInt : ValueType<int>
        {
            public TestValueTypeInt(int value) : base(value)
            {
            }
        }

        private class TestValueTypeString : ValueType<string>
        {
            public TestValueTypeString(string value) : base(value)
            {
            }
        }

        private class ExtendingTestValueTypeString : TestValueTypeString
        {
            public ExtendingTestValueTypeString(string value) : base(value)
            {
            }
        }

        private class TestObjectInt
        {
            public TestValueTypeInt ValueType { get; set; }
        }

        private class TestObjectString
        {
            public TestValueTypeString ValueType { get; set; }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _sut = new JsonValueTypeConverter();
            _serializerSettings = new JsonSerializerSettings
            {
                Converters = new[]
                {
                    _sut
                }
            };
        }

        /// <summary>
        /// CanConvert should return false when the type is <see cref="ValueType{T}"/> itself.
        /// Because the value can be read from the abstract type, but it can't be constructed.
        /// </summary>
        [TestMethod]
        public void CanConvert_ForValueType_ReturnsFalse()
        {
            // Arrange
            var type = typeof(ValueType<>).MakeGenericType(typeof(string));

            // Act
            bool result = _sut.CanConvert(type);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void CanConvert_ForClassExtendingValueType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(TestValueTypeString);

            // Act
            bool result = _sut.CanConvert(type);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void CanConvert_ForClassExtendingClassExtendingValueType_ReturnsTrue()
        {
            // Arrange
            var type = typeof(ExtendingTestValueTypeString);

            // Act
            bool result = _sut.CanConvert(type);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void CanConvert_ForOtherClass_ReturnsFalse()
        {
            // Arrange
            var type = typeof(object);

            // Act
            bool result = _sut.CanConvert(type);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WriteJson_ObjectWithStringValueType_SerializesObject()
        {
            // Arrange
            string value = "test";
            var valueType = new TestValueTypeString(value);
            var obj = new TestObjectString
            {
                ValueType = valueType
            };

            // Act
            string result = JsonConvert.SerializeObject(obj, _serializerSettings);

            // Assert
            var expectedResult = @$"{{""ValueType"":""{value}""}}";
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public void WriteJson_ObjectWithStringValueType_Null_SerializesObject()
        {
            // Arrange
            var obj = new TestObjectString
            {
                ValueType = null
            };

            // Act
            string result = JsonConvert.SerializeObject(obj, _serializerSettings);

            // Assert
            var expectedResult = @$"{{""ValueType"":null}}";
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public void WriteJson_ObjectWithIntValueType_SerializesObject()
        {
            // Arrange
            int value = 1;
            var valueType = new TestValueTypeInt(value);
            var obj = new TestObjectInt
            {
                ValueType = valueType
            };

            // Act
            string result = JsonConvert.SerializeObject(obj, _serializerSettings);

            // Assert
            var expectedResult = @$"{{""ValueType"":{value}}}";
            result.Should().Be(expectedResult);
        }

        [TestMethod]
        public void ReadJson_ObjectWithStringValueType_DeserializesObject()
        {
            // Arrange
            string value = "test";
            string json = @$"{{""ValueType"":""{value}""}}";

            // Act
            var result = JsonConvert.DeserializeObject<TestObjectString>(json, _serializerSettings);

            // Assert
            var expectedValueType = new TestValueTypeString(value);
            var expectedObject = new TestObjectString { ValueType = expectedValueType };
            result.Should().BeEquivalentTo(expectedObject);
        }

        [TestMethod]
        public void ReadJson_ObjectWithIntValueType_DeserializesObject()
        {
            // Arrange
            int value = 1;
            string json = @$"{{""ValueType"":{value}}}";

            // Act
            var result = JsonConvert.DeserializeObject<TestObjectInt>(json, _serializerSettings);

            // Assert
            var expectedValueType = new TestValueTypeInt(value);
            var expectedObject = new TestObjectInt { ValueType = expectedValueType };
            result.Should().BeEquivalentTo(expectedObject);
        }

        [TestMethod]
        public void ReadJson_ObjectWithStringValueType__IntValue_DeserializesObject()
        {
            // Arrange
            int value = 1;
            string json = @$"{{""ValueType"":{value}}}";

            // Act
            var result = JsonConvert.DeserializeObject<TestObjectString>(json, _serializerSettings);

            // Assert
            var expectedValueType = new TestValueTypeString(value.ToString());
            var expectedObject = new TestObjectString { ValueType = expectedValueType };
            result.Should().BeEquivalentTo(expectedObject);
        }
    }
}