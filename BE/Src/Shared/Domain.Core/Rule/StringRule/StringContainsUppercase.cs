using Domain.Core.Enums;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.StringRule
{
    public class StringContainsUppercase<TField> : IBusinessRule<TField> where TField : Enum
    {
        private readonly string _value;
        private readonly TField _field;

        public StringContainsUppercase(string value, TField field)
        {
            _value = value;
            _field = field;
        }

        public bool IsSatisfied() => _value?.Any(char.IsUpper) ?? false;

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.PasswordMissingUppercase;

        public IReadOnlyDictionary<object, object> Parameters
            => new Dictionary<object, object>();
    }
}
