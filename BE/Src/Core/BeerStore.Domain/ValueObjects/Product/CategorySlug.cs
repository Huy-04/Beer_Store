using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class CategorySlug : StringBase
    {
        private CategorySlug(string value) : base(value)
        {
        }

        public static CategorySlug Create(string value)
        {
            RuleValidator.CheckRules<CategoryField>(new IBusinessRule<CategoryField>[]
            {
                new StringNotEmpty<CategoryField>(value, CategoryField.Slug),
                new StringMaxLength<CategoryField>(value, 100, CategoryField.Slug),
                new StringPattern<CategoryField>(value, StringPattern<CategoryField>.DEFAULT_PATTERN, CategoryField.Slug)
            });
            return new CategorySlug(value);
        }
    }
}
