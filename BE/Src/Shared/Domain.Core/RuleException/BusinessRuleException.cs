using Domain.Core.Enums;

namespace Domain.Core.RuleException
{
    public class BusinessRuleException<TField> : Exception where TField : Enum
    {
        public ErrorCategory ErrorCategory { get; }

        public ErrorCode ErrorCode { get; }

        public TField Field { get; }

        public IReadOnlyDictionary<object, object> Parameters { get; }

        public BusinessRuleException(
            ErrorCategory errorCategory,
            TField field,
            ErrorCode errorCode,
            IReadOnlyDictionary<object, object> parameters)
        {
            Field = field;
            ErrorCategory = errorCategory;
            ErrorCode = errorCode;
            Parameters = parameters;
        }
    }
}