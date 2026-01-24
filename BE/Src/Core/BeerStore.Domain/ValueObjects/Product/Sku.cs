using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class Sku : StringBase
    {
        private Sku(string value) : base(value)
        {
        }

        public static Sku Create(string value)
        {
            RuleValidator.CheckRules<ProductVariantField>(new IBusinessRule<ProductVariantField>[]
            {
                new StringNotEmpty<ProductVariantField>(value, ProductVariantField.Sku),
                new StringMaxLength<ProductVariantField>(value, 50, ProductVariantField.Sku),
                new StringPattern<ProductVariantField>(value, @"^[a-zA-Z0-9\-_]+$", ProductVariantField.Sku)
            });
            return new Sku(value);
        }
    }
}
