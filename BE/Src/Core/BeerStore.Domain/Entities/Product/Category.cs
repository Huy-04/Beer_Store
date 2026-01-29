using BeerStore.Domain.ValueObjects.Product;
using Domain.Core.ValueObjects.Common;

namespace BeerStore.Domain.Entities.Product
{
    public class Category : AggregateRoot
    {
        public Guid? ParentId { get; private set; }

        public CategoryName Name { get; private set; }

        public CategorySlug Slug { get; private set; }

        public Description? Description { get; private set; }

        public Img? Icon { get; private set; }

        public int SortOrder { get; private set; }

        public bool IsActive { get; private set; }

        public int Level { get; private set; }

        public string Path { get; private set; }

        // Navigation
        private readonly List<Category> _children = new();
        public IReadOnlyCollection<Category> Children => _children.AsReadOnly();

        private readonly List<ProductCategory> _productCategories = new();
        public IReadOnlyCollection<ProductCategory> ProductCategories => _productCategories.AsReadOnly();

        private Category()
        { }

        private Category(
            Guid id,
            Guid? parentId,
            CategoryName name,
            CategorySlug slug,
            Description? description,
            Img? icon,
            int sortOrder,
            int level,
            string path,
            Guid createdBy,
            Guid updatedBy) : base(id)
        {
            ParentId = parentId;
            Name = name;
            Slug = slug;
            Description = description;
            Icon = icon;
            SortOrder = sortOrder;
            Level = level;
            Path = path;
            IsActive = true;
            SetCreationAudit(createdBy, updatedBy);
        }

        public static Category Create(
            Guid? parentId,
            CategoryName name,
            CategorySlug slug,
            Description? description,
            Img? icon,
            int sortOrder,
            int level,
            string path,
            Guid createdBy,
            Guid updatedBy)
        {
            return new Category(
                Guid.NewGuid(),
                parentId,
                name,
                slug,
                description,
                icon,
                sortOrder,
                level,
                path,
                createdBy,
                updatedBy);
        }

        public void UpdateName(CategoryName name)
        {
            if (Name == name) return;
            Name = name;
            Touch();
        }

        public void UpdateSlug(CategorySlug slug)
        {
            if (Slug == slug) return;
            Slug = slug;
            Touch();
        }

        public void UpdateDescription(Description? description)
        {
            Description = description;
            Touch();
        }

        public void UpdateIcon(Img? icon)
        {
            Icon = icon;
            Touch();
        }


    }
}
