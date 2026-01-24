using BeerStore.Domain.Enums.Product.Messages;
using Domain.Core.Base;
using Domain.Core.Interface.Rule;
using Domain.Core.Rule;
using Domain.Core.Rule.NumberRule;

namespace BeerStore.Domain.ValueObjects.Product
{
    /// <summary>
    /// Weight in grams for shipping calculation
    /// </summary>
    public class Weight : ValueObject
    {
        public decimal Value { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        private Weight(decimal value)
        {
            Value = value;
        }

        public static Weight Create(decimal value)
        {
            RuleValidator.CheckRules<ProductVariantField>(new IBusinessRule<ProductVariantField>[]
            {
                new NotNegativeRule<decimal, ProductVariantField>(value, ProductVariantField.Weight),
            });
            return new Weight(value);
        }
    }
}
