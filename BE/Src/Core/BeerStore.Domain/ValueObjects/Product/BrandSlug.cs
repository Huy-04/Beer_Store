using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class BrandSlug : StringBase
    {
        private BrandSlug(string value) : base(value)
        {
        }

        public static BrandSlug Create(string value)
        {
            RuleValidator.CheckRules<BrandField>(new IBusinessRule<BrandField>[]
            {
                new StringNotEmpty<BrandField>(value, BrandField.Slug),
                new StringMaxLength<BrandField>(value, 100, BrandField.Slug),
                new StringPattern<BrandField>(value, StringPattern<BrandField>.DEFAULT_PATTERN, BrandField.Slug)
            });
            return new BrandSlug(value);
        }
    }
}
