using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.EnumRule
{
    public class EnumValidateRule<TEnum, TField> : IBusinessRule<TField> where TEnum : Enum where TField : Enum
    {
        private readonly TEnum _value;
        private readonly IEnumerable<TEnum> _validate;
        private readonly TField _field;

        public EnumValidateRule(TEnum value, IEnumerable<TEnum> validValues, TField field)
        {
            _value = value;
            _validate = validValues;
            _field = field;
        }

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.InvalidStatus;

        public IReadOnlyDictionary<object, object> Parameters => new Dictionary<object, object>();

        public bool IsSatisfied() => _validate.Contains(_value);
    }
}