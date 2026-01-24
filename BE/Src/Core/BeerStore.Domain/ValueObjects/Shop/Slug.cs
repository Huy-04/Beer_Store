using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Shop
{
    public class Slug : StringBase
    {
        private Slug(string value) : base(value)
        {
        }

        public static Slug Create(string value)
        {
            RuleValidator.CheckRules<StoreField>(new IBusinessRule<StoreField>[]
            {
                new StringNotEmpty<StoreField>(value, StoreField.Slug),
                new StringMaxLength<StoreField>(value, 100, StoreField.Slug),
                new StringPattern<StoreField>(value, StringPattern<StoreField>.DEFAULT_PATTERN, StoreField.Slug)
            });
            return new Slug(value);
        }
    }
}