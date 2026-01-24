using Domain.Core.Enums;
using Domain.Core.ValueObjects.Base;

namespace Domain.Core.ValueObjects.Address
{
    public class IsDefault : EnumBase<StatusEnum>
    {
        private IsDefault(StatusEnum value) : base(value)
        {
        }

        public static IsDefault Create(StatusEnum value)
        {
            return new IsDefault(value);
        }
    }
}
