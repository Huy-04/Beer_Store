using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.NumberRule
{
    public class MaxValueLimitRule<T, TField> : IBusinessRule<TField> where T : struct, IComparable where TField : Enum
    {
        private readonly T _value;
        private readonly T _maxValue;
        private readonly TField _field;

        public MaxValueLimitRule(T value, T maxValue, TField field)
        {
            _value = value;
            _maxValue = maxValue;
            _field = field;
        }

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.ExceedsMaximum;

        public IReadOnlyDictionary<object, object> Parameters => new Dictionary<object, object>
        {
            {ParamField.Value,$"{_value} > {_maxValue}" }
        };

        public bool IsSatisfied() => _value.CompareTo(_maxValue) <= 0;
    }
}