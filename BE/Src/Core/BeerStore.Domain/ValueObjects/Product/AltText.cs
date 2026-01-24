using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.StringRule;
using Domain.Core.ValueObjects.Base;

namespace BeerStore.Domain.ValueObjects.Product
{
    public class AltText : StringBase
    {
        private AltText(string value) : base(value)
        {
        }

        public static AltText Create(string value)
        {
            RuleValidator.CheckRules<ProductImageField>(new IBusinessRule<ProductImageField>[]
            {
                new StringMaxLength<ProductImageField>(value, 125, ProductImageField.Alt),
            });
            return new AltText(value);
        }
    }
}
