using BeerStore.Domain.Enums.Product;
using BeerStore.Domain.Enums.Product.Messages;
using BeerStore.Domain.ValueObjects.Product;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using Domain.Core.ValueObjects.Common;
using Domain.Core.ValueObjects.Product;

namespace BeerStore.Domain.Entities.Product
{
    public class Product : AggregateRoot
    {
        public Guid StoreId { get; private set; }

        public Guid? BrandId { get; private set; }

        public ProductName Name { get; private set; }

        public ProductSlug Slug { get; private set; }

        public Description? Description { get; private set; }

        public ProductStatus Status { get; private set; }

        // SEO
        public MetaTitle? MetaTitle { get; private set; }

        public MetaDescription? MetaDescription { get; private set; }

        public DateTimeOffset? PublishedAt { get; private set; }

        // Pricing
        public Money BasePrice { get; private set; }

        public Money? CompareAtPrice { get; private set; }

        // Settings
        public bool HasVariants { get; private set; }

        public bool IsDigital { get; private set; }



        // Navigation
        private readonly List<ProductVariant> _variants = new();
        public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();

        private readonly List<ProductImage> _images = new();
        public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

        private readonly List<ProductCategory> _categories = new();
        public IReadOnlyCollection<ProductCategory> Categories => _categories.AsReadOnly();

        private Product()
        { }

        private Product(
            Guid id,
            Guid storeId,
            Guid? brandId,
            ProductName name,
            ProductSlug slug,
            Description? description,
            Money basePrice,
            Money? compareAtPrice,
            bool hasVariants,
            bool isDigital,
            MetaTitle? metaTitle,
            MetaDescription? metaDescription,
            Guid createdBy,
            Guid updatedBy) : base(id)
        {
            StoreId = storeId;
            BrandId = brandId;
            Name = name;
            Slug = slug;
            Description = description;
            BasePrice = basePrice;
            CompareAtPrice = compareAtPrice;
            HasVariants = hasVariants;
            IsDigital = isDigital;
            MetaTitle = metaTitle;
            MetaDescription = metaDescription;
            Status = ProductStatus.Draft;
            SetCreationAudit(createdBy, updatedBy);
        }

        public static Product Create(
            Guid storeId,
            Guid? brandId,
            ProductName name,
            ProductSlug slug,
            Description? description,
            Money basePrice,
            Money? compareAtPrice,
            bool hasVariants,
            bool isDigital,
            MetaTitle? metaTitle,
            MetaDescription? metaDescription,
            Guid createdBy,
            Guid updatedBy)
        {
            return new Product(
                Guid.NewGuid(),
                storeId,
                brandId,
                name,
                slug,
                description,
                basePrice,
                compareAtPrice,
                hasVariants,
                isDigital,
                metaTitle,
                metaDescription,
                createdBy,
                updatedBy);
        }

        #region Status Methods

        public void Publish()
        {
            if (Status == ProductStatus.Active) return;

            if (Status == ProductStatus.Deleted)
            {
                throw new BusinessRuleException<ProductField>(
                    ErrorCategory.Conflict,
                    ProductField.Status,
                    ErrorCode.InvalidStatus,
                    new Dictionary<object, object> { { "CurrentStatus", Status }, { "TargetStatus", ProductStatus.Active } });
            }

            Status = ProductStatus.Active;
            PublishedAt = DateTimeOffset.UtcNow;
            Touch();
        }

        public void Unpublish()
        {
            if (Status != ProductStatus.Active) return;

            Status = ProductStatus.Inactive;
            Touch();
        }

        public void MarkOutOfStock()
        {
            if (Status == ProductStatus.Deleted) return;

            Status = ProductStatus.OutOfStock;
            Touch();
        }

        public void Delete()
        {
            Status = ProductStatus.Deleted;
            Touch();
        }

        public void Restore()
        {
            if (Status != ProductStatus.Deleted) return;

            Status = ProductStatus.Draft;
            Touch();
        }

        #endregion Status Methods


    }
}
