using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.StringRule
{
    public class StringMinimum<TField> : IBusinessRule<TField> where TField : Enum
    {
        private readonly string _value;
        private readonly int _minimum;
        private readonly TField _field;

        public StringMinimum(string value, int minimum, TField field)
        {
            _value = value;
            _minimum = minimum;
            _field = field;
        }

        public bool IsSatisfied() => _value?.Trim().Length >= _minimum;

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.NameMinimum;

        public IReadOnlyDictionary<object, object> Parameters
            => new Dictionary<object, object>
            {
                {ParamField.Minumum.ToString(),_minimum },
            };
    }
}
