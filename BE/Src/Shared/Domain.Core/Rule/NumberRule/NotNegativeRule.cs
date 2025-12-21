using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.NumberRule
{
    public class NotNegativeRule<T, TField> : IBusinessRule<TField> where T : struct, IComparable where TField : Enum
    {
        private readonly T _value;
        private readonly TField _field;

        public NotNegativeRule(T value, TField field)
        {
            _value = value;
            _field = field;
        }

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.NotNegative;

        public IReadOnlyDictionary<object, object> Parameters => new Dictionary<object, object>
        {
            {ParamField.Value,_value }
        };

        public bool IsSatisfied() => _value.CompareTo(default(T)) >= 0;
    }
}