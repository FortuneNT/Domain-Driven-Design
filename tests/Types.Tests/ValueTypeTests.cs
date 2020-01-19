using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DDD.Types.Tests
{
    [TestClass]
    public class ValueTypeTests
    {
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

        [TestMethod]
        public void ValueType_WhenConstructedWithDefault_ThrowsArgumentNullExceptions()
        {
            // Act
            Action act = () => new TestValueTypeInt(default);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void ValueType_WhenConstructedWithNull_ThrowsArgumentNullExceptions()
        {
            // Act
            Action act = () => new TestValueTypeString(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void EqualsObject_WhenCalledWithNull_ReturnsFalse()
        {
            // Arrange
            var obj = new TestValueTypeInt(1);

            // Act
            var result = obj.Equals((object)null);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void EqualsObject_WhenCalledWithTheSameReferenceObject_ReturnsTrue()
        {
            // Arrange
            var obj = new TestValueTypeInt(1);

            // Act
            var result = obj.Equals(obj);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void EqualsObject_WhenCalledWithADifferentObject_ReturnsFalse()
        {
            // Arrange
            var object1 = new TestValueTypeInt(1);
            var object2 = 1;

            // Act
            var result = object1.Equals((object)object2);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void EqualsObject_WhenCalledWithADifferentObjectWhichIsTheSame_ReturnsTrue()
        {
            // Arrange
            var object1 = new TestValueTypeInt(1);
            var object2 = new TestValueTypeInt(1);

            // Act
            var result = object1.Equals((object)object2);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Equals_WhenCalledWithNull_ReturnsFalse()
        {
            // Arrange
            var object1 = new TestValueTypeInt(1);

            // Act
            var result = object1.Equals(null);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Equals_WhenCalledWithTheSameReferenceObject_ReturnsTrue()
        {
            // Arrange
            var object1 = new TestValueTypeInt(1);

            // Act
            var result = object1.Equals(object1);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Equals_WhenCalledWithADifferentObjectWhichIsTheSame_ReturnsTrue()
        {
            // Arrange
            var object1 = new TestValueTypeInt(1);
            var object2 = new TestValueTypeInt(1);

            // Act
            var result = object1.Equals(object2);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Equals_WhenCalledWithADifferentObjectWhichIsNotTheSame_ReturnsFalse()
        {
            // Arrange
            var object1 = new TestValueTypeInt(1);
            var object2 = new TestValueTypeInt(2);

            // Act
            var result = object1.Equals(object2);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void EqualsOperator_WhenCalledWithTheSameReferenceObject_CallsEqualWithThatObject_WhichReturnsTrue()
        {
            // Arrange
            var object1 = new TestValueTypeInt(1);
            var object2 = object1;

            // Act
            var result = object1 == object2;

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void NotEqualsOperator_WhenCalledWithTheSameReferenceObject_CallsEqualWithThatObject_WhichReturnsFalse()
        {
            // Arrange
            var object1 = new TestValueTypeInt(1);
            var object2 = object1;

            // Act
            var result = object1 != object2;

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void ToString_WhenCalled_ReturnsAString()
        {
            // Arrange
            var value = 1;
            var obj = new TestValueTypeInt(value);

            // Act
            var result = obj.ToString();

            // Assert
            result.Should().Be(value.ToString());
        }

        [TestMethod]
        public void GetHashCode_WhenCalledOnAValidObject_ReturnsANonZeroHashCode()
        {
            // Arrange
            var obj = new TestValueTypeInt(1);

            // Act
            var result = obj.GetHashCode();

            // Assert
            Assert.IsNotNull(result);
            result.Should().NotBe(0);
        }

        [TestMethod]
        public void GetHashCode_WhenCalledOnTheSameObject_ReturnsTheSameHashCodes()
        {
            // Arrange
            var object1 = new TestValueTypeInt(1);

            // Act
            var result1 = object1.GetHashCode();
            var result2 = object1.GetHashCode();

            // Assert
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            result1.Should().NotBe(0);
            result1.Should().NotBe(0);
            result1.Should().Be(result2);
        }

        [TestMethod]
        public void GetHashCode_WhenCalledOnDifferentValidObjects_ReturnsDifferentHashCodes()
        {
            // Arrange
            var object1 = new TestValueTypeInt(1);
            var object2 = new TestValueTypeInt(2);

            // Act
            var result1 = object1.GetHashCode();
            var result2 = object2.GetHashCode();

            // Assert
            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            result1.Should().NotBe(0);
            result1.Should().NotBe(0);
            result1.Should().NotBe(result2);
        }
    }
}