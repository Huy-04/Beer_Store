using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class BrandName : StringBase
    {
        private BrandName(string value) : base(value)
        {
        }

        public static BrandName Create(string value)
        {
            RuleValidator.CheckRules<BrandField>(new IBusinessRule<BrandField>[]
            {
                new StringNotEmpty<BrandField>(value, BrandField.Name),
                new StringMaxLength<BrandField>(value, 100, BrandField.Name),
            });
            return new BrandName(value);
        }
    }
}
