using System;

namespace NitroxModel.DataStructures.Util
{
    public class Optional<T>
    {
        public T Value { get; }

        public bool HasValue { get; }

        private Optional()
        {
        }

        private Optional(T value)
        {
            Value = value;
            HasValue = true;
        }

        public static Optional<T> Empty()
        {
            return new Optional<T>();
        }

        public static Optional<T> Of(T value)
        {
            if (value == null || value.Equals(default(T)))
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be default");
            }

            return new Optional<T>(value);
        }

        public static Optional<T> OfNullable(T value)
        {
            if (value == null || value.Equals(default(T)))
            {
                return new Optional<T>();
            }

            return new Optional<T>(value);
        }

        public T Get()
        {
            if (!HasValue)
            {
                throw new OptionalEmptyException<T>();
            }

            return Value;
        }

        public T OrElse(T elseValue)
        {
            if (HasValue)
            {
                return Value;
            }
            else
            {
                return elseValue;
            }
        }
    }
    
    [Serializable]
    public sealed class OptionalEmptyException<T> : Exception
    {
        public OptionalEmptyException() : base($"Optional <{nameof(T)}> is empty.")
        {
        }

        public OptionalEmptyException(string message) : base($"Optional <{nameof(T)}> is empty:\n\t{message}")
        {
        }
    }
}
