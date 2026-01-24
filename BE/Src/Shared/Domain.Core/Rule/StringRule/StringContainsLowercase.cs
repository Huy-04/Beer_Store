using Domain.Core.Enums;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.StringRule
{
    public class StringContainsLowercase<TField> : IBusinessRule<TField> where TField : Enum
    {
        private readonly string _value;
        private readonly TField _field;

        public StringContainsLowercase(string value, TField field)
        {
            _value = value;
            _field = field;
        }

        public bool IsSatisfied() => string.IsNullOrEmpty(_value) || _value.Any(char.IsLower);

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.PasswordMissingLowercase;

        public IReadOnlyDictionary<object, object> Parameters
            => new Dictionary<object, object>();
    }
}
