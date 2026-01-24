using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class CategoryName : StringBase
    {
        private CategoryName(string value) : base(value)
        {
        }

        public static CategoryName Create(string value)
        {
            RuleValidator.CheckRules<CategoryField>(new IBusinessRule<CategoryField>[]
            {
                new StringNotEmpty<CategoryField>(value, CategoryField.Name),
                new StringMaxLength<CategoryField>(value, 100, CategoryField.Name),
            });
            return new CategoryName(value);
        }
    }
}
