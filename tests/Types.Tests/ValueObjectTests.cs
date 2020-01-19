using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DDD.Types.Tests
{
    [TestClass]
    public class ValueObjectTest
    {
        private class TestValueObject : ValueObject<TestValueObject>
        {
            private readonly int x;
            private readonly int y;

            public TestValueObject(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            protected override IEnumerable<object> GetAtomicValues()
            {
                yield return x;
                yield return y;
            }
        }

        private class TestValueObjectExtend : TestValueObject
        {
            private readonly int z;

            public TestValueObjectExtend(int x, int y, int z) : base(x, y)
            {
                this.z = z;
            }

            protected override IEnumerable<object> GetAtomicValues()
            {
                foreach (var atomicValue in base.GetAtomicValues())
                {
                    yield return atomicValue;
                }

                yield return z;
            }
        }

        [TestMethod]
        public void EqualsObject_WhenCalledWithNull_ReturnsFalse()
        {
            // Arrange
            var obj = new TestValueObject(1, 1);

            // Act
            var result = obj.Equals((object)null);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void EqualsObject_WhenCalledWithTheSameReferenceObject_ReturnsTrue()
        {
            // Arrange
            var obj = new TestValueObject(1, 1);

            // Act
            var result = obj.Equals((object)obj);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void EqualsObject_WhenCalledWithADifferentObject_ReturnsFalse()
        {
            // Arrange
            var object1 = new TestValueObject(1, 1);
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
            var object1 = new TestValueObject(1, 1);
            var object2 = new TestValueObject(1, 1);

            // Act
            var result = object1.Equals((object)object2);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Equals_WhenCalledWithNull_ReturnsFalse()
        {
            // Arrange
            var object1 = new TestValueObject(1, 1);

            // Act
            var result = object1.Equals(null);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Equals_WhenCalledWithTheSameReferenceObject_ReturnsTrue()
        {
            // Arrange
            var object1 = new TestValueObject(1, 1);

            // Act
            var result = object1.Equals(object1);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Equals_WhenCalledWithADifferentObjectWhichIsTheSame_ReturnsTrue()
        {
            // Arrange
            var object1 = new TestValueObject(1, 1);
            var object2 = new TestValueObject(1, 1);

            // Act
            var result = object1.Equals(object2);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Equals_WhenCalledWithADifferentObjectWhichIsNotTheSame_ReturnsFalse()
        {
            // Arrange
            var object1 = new TestValueObject(1, 1);
            var object2 = new TestValueObject(1, 2);

            // Act
            var result = object1.Equals(object2);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Equals_WhenCalledWithADifferentObjectWhichExtendsTheFirstWithAdditionalProperties_WithEqualBaseValues_ReturnsFalse()
        {
            // Arrange
            var object1 = new TestValueObject(1, 1);
            var object2 = new TestValueObjectExtend(1, 1, 1);

            // Act
            var result = object1.Equals(object2);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Equals_WhenCalledWithADifferentObjectWhichExtendsTheFirstWithAdditionalProperties_WithInEqualBaseValues_ReturnsFalse()
        {
            // Arrange
            var object1 = new TestValueObject(1, 1);
            var object2 = new TestValueObjectExtend(2, 2, 2);

            // Act
            var result = object1.Equals(object2);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void EqualsOperator_WhenCalledWithTheSameReferenceObject_ReturnsTrue()
        {
            // Arrange
            var object1 = new TestValueObject(1, 1);
            var object2 = object1;

            // Act
            var result = object1 == object2;

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void NotEqualsOperator_WhenCalledWithTheSameReferenceObject_ReturnsFalse()
        {
            // Arrange
            var object1 = new TestValueObject(1, 1);
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
            var value1 = 1;
            var value2 = 2;
            var object1 = new TestValueObject(value1, value2);

            // Act
            var result = object1.ToString();

            // Assert
            result.Should().Be($"{nameof(TestValueObject)}({value1}, {value2})");
        }

        [TestMethod]
        public void GetHashCode_WhenCalledOnAValidObject_ReturnsANonZeroHashCode()
        {
            // Arrange
            var object1 = new TestValueObject(1, 2);

            // Act
            var result = object1.GetHashCode();

            // Assert
            Assert.IsNotNull(result);
            result.Should().NotBe(0);
        }

        [TestMethod]
        public void GetHashCode_WhenCalledOnTheSameObject_ReturnsTheSameHashCodes()
        {
            // Arrange
            var object1 = new TestValueObject(1, 2);

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
            var object1 = new TestValueObject(1, 2);
            var object2 = new TestValueObject(3, 4);

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

        [Ignore]
        [TestMethod]
        public void GetHashCode_WhenCalledOnAValidObject_With2IdenticalValues_ReturnsANonZeroHashCode()
        {
            // Arrange
            var object1 = new TestValueObject(1, 1);

            // Act
            var result = object1.GetHashCode();

            // Assert
            Assert.IsNotNull(result);
            result.Should().NotBe(0);
        }

        [Ignore]
        [TestMethod]
        public void GetHashCode_WhenCalledOnDifferentValidObjects_WithTheParametersSwapped_ReturnsDifferentHashCodes()
        {
            // Arrange
            var object1 = new TestValueObject(1, 2);
            var object2 = new TestValueObject(2, 1);

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