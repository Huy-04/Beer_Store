using Domain.Core.Base;
using Domain.Core.Interface.Rule;
using Domain.Core.Enums.Messages;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;

namespace Domain.Core.ValueObjects
{
    public sealed class Img : ValueObject
    {
        public string Value { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        private Img(string value)
        {
            Value = value;
        }

        public static Img Create(string value)
        {
            RuleValidator.CheckRules<CommonField>(new IBusinessRule<CommonField>[]
            {
                new StringMaxLength<CommonField>(value, 255, CommonField.Img),
                new StringNotEmpty<CommonField>(value, CommonField.Img),
                new StringMinimum<CommonField>(value, 5, CommonField.Img),
            });
            return new Img(value);
        }

        public static implicit operator string(Img img) => img.Value;

        public override string ToString() => Value;
    }
}
