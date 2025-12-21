using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.Interface.Rule;

namespace Domain.Core.Rule.EnumRule
{
    public class EnumEqualRule<TEnum, TField> : IBusinessRule<TField> where TEnum : Enum where TField : Enum
    {
        private readonly TEnum _left;
        private readonly TEnum _right;
        private readonly TField _field;

        public EnumEqualRule(TEnum left, TEnum right, TField field)
        {
            _left = left;
            _right = right;
            _field = field;
        }

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.TypeMismatch;

        public IReadOnlyDictionary<object, object> Parameters => new Dictionary<object, object>
        {
            {ParamField.Value,$"{_left} != {_right}" }
        };

        public bool IsSatisfied() => _left.Equals(_right);
    }
}