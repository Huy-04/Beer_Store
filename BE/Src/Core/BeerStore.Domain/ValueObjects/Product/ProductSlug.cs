using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class ProductSlug : StringBase
    {
        private ProductSlug(string value) : base(value)
        {
        }

        public static ProductSlug Create(string value)
        {
            RuleValidator.CheckRules<ProductField>(new IBusinessRule<ProductField>[]
            {
                new StringNotEmpty<ProductField>(value, ProductField.Slug),
                new StringMaxLength<ProductField>(value, 200, ProductField.Slug),
                new StringPattern<ProductField>(value, StringPattern<ProductField>.DEFAULT_PATTERN, ProductField.Slug)
            });
            return new ProductSlug(value);
        }
    }
}
