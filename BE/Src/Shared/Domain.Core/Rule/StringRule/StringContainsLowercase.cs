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

        public bool IsSatisfied() => _value?.Any(char.IsLower) ?? false;

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.PasswordMissingLowercase;

        public IReadOnlyDictionary<object, object> Parameters
            => new Dictionary<object, object>();
    }
}
