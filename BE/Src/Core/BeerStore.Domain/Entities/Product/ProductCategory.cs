using Domain.Core.Base;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.Entities.Product
{
    public class ProductCategory : Entity
    {
        public Guid ProductId { get; private set; }

        public Guid CategoryId { get; private set; }

        public bool IsPrimary { get; private set; }

        private ProductCategory()
        { }

        private ProductCategory(Guid id, Guid productId, Guid categoryId, bool isPrimary) : base(id)
        {
            ProductId = productId;
            CategoryId = categoryId;
            IsPrimary = isPrimary;
        }

        public static ProductCategory Create(Guid productId, Guid categoryId, bool isPrimary = false)
        {
            return new ProductCategory(Guid.NewGuid(), productId, categoryId, isPrimary);
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
