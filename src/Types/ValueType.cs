using System;

namespace DDD.Types
{
    public abstract class ValueType<T> : IEquatable<ValueType<T>>
        where T : IEquatable<T>
    {
        // The setter is private otherwise EntityFramework can't assign the value after construction
        protected T Value { get; private set; }

        public ValueType<T> GetCopy() => MemberwiseClone() as ValueType<T>;

        protected ValueType(T value)
        {
            if (value == null || value.Equals(default)) throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        public bool Equals(ValueType<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ValueType<T> other && Equals(other);
        }

        public override int GetHashCode()
            => Value != null ? Value.GetHashCode() : 0;

        public static bool operator ==(ValueType<T> left, ValueType<T> right)
            => Equals(left, right);

        public static bool operator !=(ValueType<T> left, ValueType<T> right)
            => !Equals(left, right);

        public override string ToString() 
            => Value.ToString();
    }
}