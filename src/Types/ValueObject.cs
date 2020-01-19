using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Types
{
    // Based on the ValueObject from the dotnet-architecture eSopOnContainers repository
    // https://github.com/dotnet-architecture/eShopOnContainers/blob/1b7200791931f33c94206822a69644ca820bb0dc/src/Services/Ordering/Ordering.Domain/SeedWork/ValueObject.cs
    public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
        where T : ValueObject<T>
    {
        protected abstract IEnumerable<object> GetAtomicValues();

        public ValueObject<T> GetCopy() => MemberwiseClone() as ValueObject<T>;

        public bool Equals(ValueObject<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (thisValues.Current is null ^ otherValues.Current is null)
                {
                    return false;
                }
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ValueObject<T> other && Equals(other);
        }

        /// <summary>
        /// Known bugs for which it returns a wrong/the same HashCode:
        /// <para>- It returns a zero HashCode for an object with 2 identical values</para>
        /// <para>- It returns the same HashCode for objects with the same property values even though it are different properties</para>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
            => Equals(left, right);

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
            => !Equals(left, right);
    }
}