using Domain.Core.Base;

namespace Domain.Core.ValueObjects
{
    public abstract class StringBase : ValueObject
    {
        public string Value { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        protected StringBase(string value)
        {
            Value = value;
        }

        public static implicit operator string(StringBase name) => name.Value;

        public override string ToString() => Value;
    }
}