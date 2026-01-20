using Domain.Core.Enums;
using Domain.Core.RuleException;

namespace Domain.Core.Helpers
{
    public static class AuthorizationHelper
    {
        public static void ThrowForbidden<TField>(TField field) where TField : Enum
        {
            throw new BusinessRuleException<TField>(
                ErrorCategory.Forbidden,
                field,
                ErrorCode.AccessDenied,
                new Dictionary<object, object>());
        }
    }
}
