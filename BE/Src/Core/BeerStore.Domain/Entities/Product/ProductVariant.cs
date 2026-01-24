using BeerStore.Domain.ValueObjects.Product;
using Domain.Core.Base;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.Entities.Product
{
    public class ProductVariant : Entity
    {
        public Guid ProductId { get; private set; }

        public Sku Sku { get; private set; }

        public VariantName? Name { get; private set; }

        // Pricing
        public Money Price { get; private set; }

        public Money? CompareAtPrice { get; private set; }

        // Physical
        public Weight? Weight { get; private set; }

        // Options
        public VariantOption? Option1 { get; private set; }

        public VariantOption? Option2 { get; private set; }

        public VariantOption? Option3 { get; private set; }

        public bool IsActive { get; private set; }

        // Audit
        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        private ProductVariant()
        { }

        private ProductVariant(
            Guid id,
            Guid productId,
            Sku sku,
            VariantName? name,
            Money price,
            Money? compareAtPrice,
            Weight? weight,
            VariantOption? option1,
            VariantOption? option2,
            VariantOption? option3) : base(id)
        {
            ProductId = productId;
            Sku = sku;
            Name = name;
            Price = price;
            CompareAtPrice = compareAtPrice;
            Weight = weight;
            Option1 = option1;
            Option2 = option2;
            Option3 = option3;
            IsActive = true;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static ProductVariant Create(
            Guid productId,
            Sku sku,
            VariantName? name,
            Money price,
            Money? compareAtPrice,
            Weight? weight,
            VariantOption? option1,
            VariantOption? option2,
            VariantOption? option3)
        {
            return new ProductVariant(
                Guid.NewGuid(),
                productId,
                sku,
                name,
                price,
                compareAtPrice,
                weight,
                option1,
                option2,
                option3);
        }

        public void UpdateSku(Sku sku)
        {
            if (Sku == sku) return;
            Sku = sku;
            Touch();
        }

        public void UpdateName(VariantName? name)
        {
            Name = name;
            Touch();
        }

        public void UpdatePrice(Money price)
        {
            if (Price == price) return;
            Price = price;
            Touch();
        }

        public void UpdateCompareAtPrice(Money? compareAtPrice)
        {
            CompareAtPrice = compareAtPrice;
            Touch();
        }

        public void UpdateWeight(Weight? weight)
        {
            Weight = weight;
            Touch();
        }

        public void UpdateOptions(VariantOption? option1, VariantOption? option2, VariantOption? option3)
        {
            Option1 = option1;
            Option2 = option2;
            Option3 = option3;
            Touch();
        }

        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
            Touch();
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
            Touch();
        }

        public void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
    }
}
