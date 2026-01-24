using Domain.Core.Enums;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.StringRule
{
    public class StringNotEmpty<TField> : IBusinessRule<TField> where TField : Enum
    {
        private readonly string _value;
        private readonly TField _field;

        public StringNotEmpty(string value, TField field)
        {
            _value = value;
            _field = field;
        }

        public bool IsSatisfied() => !string.IsNullOrEmpty(_value?.Trim());

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.NameEmpty;

        public IReadOnlyDictionary<object, object> Parameters => new Dictionary<object, object>();
    }
}