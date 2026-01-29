using BeerStore.Domain.ValueObjects.Product;
using Domain.Core.Base;
using Domain.Core.ValueObjects.Common;

namespace BeerStore.Domain.Entities.Product
{
    public class ProductImage : Entity
    {
        public Guid ProductId { get; private set; }

        public Guid? VariantId { get; private set; }

        public Img Url { get; private set; }

        public AltText? Alt { get; private set; }

        public int SortOrder { get; private set; }

        public bool IsPrimary { get; private set; }

        private ProductImage()
        { }

        private ProductImage(
            Guid id,
            Guid productId,
            Guid? variantId,
            Img url,
            AltText? alt,
            int sortOrder,
            bool isPrimary) : base(id)
        {
            ProductId = productId;
            VariantId = variantId;
            Url = url;
            Alt = alt;
            SortOrder = sortOrder;
            IsPrimary = isPrimary;
        }

        public static ProductImage Create(
            Guid productId,
            Guid? variantId,
            Img url,
            AltText? alt,
            int sortOrder,
            bool isPrimary)
        {
            return new ProductImage(
                Guid.NewGuid(),
                productId,
                variantId,
                url,
                alt,
                sortOrder,
                isPrimary);
        }

        public void UpdateUrl(Img url)
        {
            if (Url == url) return;
            Url = url;
        }

        public void UpdateAlt(AltText? alt)
        {
            Alt = alt;
        }

        public void UpdateSortOrder(int sortOrder)
        {
            SortOrder = sortOrder;
        }

        public void SetAsPrimary()
        {
            IsPrimary = true;
        }

        public void RemoveAsPrimary()
        {
            IsPrimary = false;
        }
    }
}
