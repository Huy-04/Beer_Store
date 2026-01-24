using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class MetaDescription : StringBase
    {
        private MetaDescription(string value) : base(value)
        {
        }

        public static MetaDescription Create(string value)
        {
            RuleValidator.CheckRules<ProductField>(new IBusinessRule<ProductField>[]
            {
                new StringMaxLength<ProductField>(value, 160, ProductField.MetaDescription),
            });
            return new MetaDescription(value);
        }
    }
}
