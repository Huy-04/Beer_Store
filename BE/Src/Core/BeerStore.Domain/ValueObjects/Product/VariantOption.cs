using Domain.Core.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    /// <summary>
    /// Represents a variant option like { Name: "Color", Value: "Red" }
    /// </summary>
    public class VariantOption : ValueObject
    {
        public string Name { get; }
        public string Value { get; }

        private VariantOption(string name, string value)
        {
            Name = name;
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Value;
        }

        public static VariantOption Create(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Option name cannot be empty", nameof(name));
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Option value cannot be empty", nameof(value));
            if (name.Length > 50)
                throw new ArgumentException("Option name cannot exceed 50 characters", nameof(name));
            if (value.Length > 100)
                throw new ArgumentException("Option value cannot exceed 100 characters", nameof(value));

            return new VariantOption(name.Trim(), value.Trim());
        }
    }
}
