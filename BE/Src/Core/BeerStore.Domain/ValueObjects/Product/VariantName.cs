using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class VariantName : StringBase
    {
        private VariantName(string value) : base(value)
        {
        }

        public static VariantName Create(string value)
        {
            RuleValidator.CheckRules<ProductVariantField>(new IBusinessRule<ProductVariantField>[]
            {
                new StringMaxLength<ProductVariantField>(value, 100, ProductVariantField.Name),
            });
            return new VariantName(value);
        }
    }
}
