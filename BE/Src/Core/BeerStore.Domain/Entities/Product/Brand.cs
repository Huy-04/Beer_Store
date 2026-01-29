using BeerStore.Domain.ValueObjects.Product;
using Domain.Core.ValueObjects.Common;

namespace BeerStore.Domain.Entities.Product
{
    public class Brand : AggregateRoot
    {
        public BrandName Name { get; private set; }

        public BrandSlug Slug { get; private set; }

        public Description? Description { get; private set; }

        public Img? Logo { get; private set; }

        public bool IsActive { get; private set; }

        public bool IsVerified { get; private set; }



        private Brand()
        { }

        private Brand(
            Guid id,
            BrandName name,
            BrandSlug slug,
            Description? description,
            Img? logo,
            Guid createdBy,
            Guid updatedBy) : base(id)
        {
            Name = name;
            Slug = slug;
            Description = description;
            Logo = logo;
            IsActive = true;
            IsVerified = false;
            SetCreationAudit(createdBy, updatedBy);
        }

        public static Brand Create(
            BrandName name,
            BrandSlug slug,
            Description? description,
            Img? logo,
            Guid createdBy,
            Guid updatedBy)
        {
            return new Brand(
                Guid.NewGuid(),
                name,
                slug,
                description,
                logo,
                createdBy,
                updatedBy);
        }

        public void UpdateName(BrandName name)
        {
            if (Name == name) return;
            Name = name;
            Touch();
        }

        public void UpdateSlug(BrandSlug slug)
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

        public void UpdateLogo(Img? logo)
        {
            Logo = logo;
            Touch();
        }



        public void Verify()
        {
            if (IsVerified) return;
            IsVerified = true;
            Touch();
        }

        public void Unverify()
        {
            if (!IsVerified) return;
            IsVerified = false;
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


    }
}
