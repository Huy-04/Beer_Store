using Domain.Core.Enums;
using Domain.Core.Interface.Rule;
using System.Text.RegularExpressions;

namespace Domain.Core.Rule.StringRule
{
    public class StringPattern<TField> : IBusinessRule<TField> where TField : Enum
    {
        private readonly string _value;
        private readonly string _pattern;
        private readonly TField _field;

        public const string DEFAULT_PATTERN = @"^[a-z0-9]+(-[a-z0-9]+)*$";

        public StringPattern(string value, string pattern, TField field)
        {
            _value = value;
            _pattern = pattern;
            _field = field;
        }

        public bool IsSatisfied() => string.IsNullOrEmpty(_value) || Regex.IsMatch(_value, _pattern);

        public TField Field => _field;

        public ErrorCode Error => ErrorCode.InvalidFormat;

        public IReadOnlyDictionary<object, object> Parameters =>
            new Dictionary<object, object>
            {
                { "Pattern", _pattern },
                { "Value", _value }
            };
    }
}