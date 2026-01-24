using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class MetaTitle : StringBase
    {
        private MetaTitle(string value) : base(value)
        {
        }

        public static MetaTitle Create(string value)
        {
            RuleValidator.CheckRules<ProductField>(new IBusinessRule<ProductField>[]
            {
                new StringMaxLength<ProductField>(value, 60, ProductField.MetaTitle),
            });
            return new MetaTitle(value);
        }
    }
}
