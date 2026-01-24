using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Shop
{
    public class StoreName : StringBase
    {
        private StoreName(string value) : base(value)
        {
        }

        public static StoreName Create(string value)
        {
            RuleValidator.CheckRules<StoreField>(new IBusinessRule<StoreField>[]
            {
                new StringNotEmpty<StoreField>(value, StoreField.Name),
                new StringMaxLength<StoreField>(value, 100, StoreField.Name)
            });
            return new StoreName(value);
        }
    }
}
