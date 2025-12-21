using Domain.Core.Base;
using Domain.Core.Interface.Rule;
using Domain.Core.Enums.Messages;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;

namespace Domain.Core.ValueObjects
{
    public sealed class Description : ValueObject
    {
        public string Value { get; }

        private Description(string value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static Description Create(string value)
        {
            RuleValidator.CheckRules<CommonField>(new IBusinessRule<CommonField>[]
            {
                new StringMaxLength<CommonField>(value, 255, CommonField.Description),
                new StringNotEmpty<CommonField>(value, CommonField.Description),
                new StringMinimum<CommonField>(value, 5, CommonField.Description),
            });
            return new Description(value);
        }

        public static implicit operator string(Description description) => description.Value;

        public override string ToString() => Value;
    }
}
