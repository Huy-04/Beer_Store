using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class ProductName : StringBase
    {
        private ProductName(string value) : base(value)
        {
        }

        public static ProductName Create(string value)
        {
            RuleValidator.CheckRules<ProductField>(new IBusinessRule<ProductField>[]
            {
                new StringNotEmpty<ProductField>(value, ProductField.Name),
                new StringMaxLength<ProductField>(value, 200, ProductField.Name),
            });
            return new ProductName(value);
        }
    }
}
