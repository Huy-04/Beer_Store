using BeerStore.Domain.Enums.Shop;
using BeerStore.Domain.Enums.Shop.Messages;
using BeerStore.Domain.ValueObjects.Shop;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using Domain.Core.ValueObjects;

namespace BeerStore.Domain.Entities.Shop
{
    public class Store : AggregateRoot
    {
        public Guid OwnerId { get; private set; }

        public StoreName Name { get; private set; }

        public Slug Slug { get; private set; }

        public Img Logo { get; private set; }

        public Description Description { get; private set; }

        public StoreType StoreType { get; private set; }

        public StoreStatus StoreStatus { get; private set; }

        public Guid CreatedBy { get; private set; }

        public Guid UpdatedBy { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        public bool IsOfficial => StoreType == StoreType.OfficialStore && StoreStatus == StoreStatus.Approved;

        private Store()
        { }

        private Store(Guid id, Guid ownerId, StoreName name, Slug slug, StoreType type, Img logo, Description description, Guid createdBy, Guid updatedBy)
            : base(id)
        {
            OwnerId = ownerId;
            Name = name;
            Slug = slug;
            StoreType = type;
            Logo = logo;
            Description = description;
            StoreStatus = StoreStatus.Pending;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static Store Create(Guid ownerId, StoreName name, Slug slug, StoreType type, Img logo, Description description, Guid createdBy, Guid updatedBy)
        {
            var shop = new Store(Guid.NewGuid(), ownerId, name, slug, type, logo, description, createdBy, updatedBy);
            return shop;
        }

        public void UpdateName(StoreName name)
        {
            if (Name == name) return;
            Name = name;
            Touch();
        }

        public void UpdateSlug(Slug slug)
        {
            if (Slug == slug) return;
            Slug = slug;
            Touch();
        }

        public void UpdateLogo(Img logo)
        {
            if (Logo == logo) return;
            Logo = logo;
            Touch();
        }

        public void UpdateDescription(Description description)
        {
            if (Description == description) return;
            Description = description;
            Touch();
        }

        public void SetUpdatedBy(Guid updatedBy)
        {
            if (UpdatedBy == updatedBy) return;
            UpdatedBy = updatedBy;
            Touch();
        }

        // State Machine Methods
        public void Approve()
        {
            EnsureStatusTransition(StoreStatus.Pending, StoreStatus.Approved);
            StoreStatus = StoreStatus.Approved;
            Touch();
        }

        public void Reject()
        {
            EnsureStatusTransition(StoreStatus.Pending, StoreStatus.Rejected);
            StoreStatus = StoreStatus.Rejected;
            Touch();
        }

        public void Suspend()
        {
            EnsureStatusTransition(StoreStatus.Approved, StoreStatus.Suspended);
            StoreStatus = StoreStatus.Suspended;
            Touch();
        }

        public void Reactivate()
        {
            EnsureStatusTransition(StoreStatus.Suspended, StoreStatus.Approved);
            StoreStatus = StoreStatus.Approved;
            Touch();
        }

        public void Resubmit()
        {
            EnsureStatusTransition(StoreStatus.Rejected, StoreStatus.Pending);
            StoreStatus = StoreStatus.Pending;
            Touch();
        }

        public void Ban()
        {
            if (StoreStatus == StoreStatus.Banned)
            {
                throw new BusinessRuleException<StoreField>(
                    ErrorCategory.Conflict,
                    StoreField.StoreStatus,
                    ErrorCode.InvalidStatusTransition,
                    new Dictionary<object, object>
                    {
                        { "CurrentStatus", StoreStatus },
                        { "TargetStatus", StoreStatus.Banned }
                    });
            }
            StoreStatus = StoreStatus.Banned;
            Touch();
        }

        private void EnsureStatusTransition(StoreStatus requiredStatus, StoreStatus targetStatus)
        {
            if (StoreStatus != requiredStatus)
            {
                throw new BusinessRuleException<StoreField>(
                    ErrorCategory.Conflict,
                    StoreField.StoreStatus,
                    ErrorCode.InvalidStatusTransition,
                    new Dictionary<object, object>
                    {
                        { "CurrentStatus", StoreStatus },
                        { "RequiredStatus", requiredStatus },
                        { "TargetStatus", targetStatus }
                    });
            }
        }

        public void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}