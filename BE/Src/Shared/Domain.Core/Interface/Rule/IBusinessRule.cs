using Domain.Core.Enums;

namespace Domain.Core.Interface.Rule
{
    public interface IBusinessRule<TField> where TField : Enum
    {
        ErrorCode Error { get; }

        TField Field { get; }

        IReadOnlyDictionary<object, object> Parameters { get; }

        bool IsSatisfied();
    }
}