using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.StringRule
{
    public class StringMaxLength<TField> : IBusinessRule<TField> where TField : Enum
    {
        private readonly string _value;
        private readonly TField _field;
        private readonly int _maxlength;

        public StringMaxLength(string value, int maxlength, TField field)
        {
            _value = value;
            _field = field;
            _maxlength = maxlength;
        }

        public bool IsSatisfied() => _value?.Trim().Length <= _maxlength;

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.NameTooLong;

        public IReadOnlyDictionary<object, object> Parameters
            => new Dictionary<object, object>
            {
                {ParamField.MaxLength,_maxlength },
            };
    }
}