using BeerStore.Domain.Enums.Shop;
using BeerStore.Domain.Enums.Shop.Messages;
using BeerStore.Domain.ValueObjects.Shop;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using Domain.Core.ValueObjects.Common;
using BeerStore.Domain.Entities.Shop.Junction;

namespace BeerStore.Domain.Entities.Shop
{
    public class Store : AggregateRoot
    {
        public Guid OwnerId { get; private set; }

        public StoreName StoreName { get; private set; }

        public Slug Slug { get; private set; }

        public Img Logo { get; private set; }

        public Description Description { get; private set; }

        public StoreType StoreType { get; private set; }

        public StoreStatus StoreStatus { get; private set; }

        private readonly List<StoreAddress> _storeAddresses = new List<StoreAddress>();
        public IReadOnlyCollection<StoreAddress> StoreAddresses => _storeAddresses.AsReadOnly();

        public bool IsOfficial => StoreType == StoreType.OfficialStore && StoreStatus == StoreStatus.Approved;

        private Store()
        { }

        private Store(Guid id, Guid ownerId, StoreName storeName, Slug slug, StoreType type, Img logo, Description description, Guid createdBy, Guid updatedBy)
            : base(id)
        {
            OwnerId = ownerId;
            StoreName = storeName;
            Slug = slug;
            StoreType = type;
            Logo = logo;
            Description = description;
            StoreStatus = StoreStatus.Pending;
            SetCreationAudit(createdBy, updatedBy);
        }

        public static Store Create(Guid ownerId, StoreName storeName, Slug slug, StoreType type, Img logo, Description description, Guid createdBy, Guid updatedBy)
        {
            var shop = new Store(Guid.NewGuid(), ownerId, storeName, slug, type, logo, description, createdBy, updatedBy);
            return shop;
        }

        public void UpdateName(StoreName storeName)
        {
            if (StoreName == storeName) return;
            StoreName = storeName;
            Touch();
        }

        // Address Management
        public void AddAddress(StoreAddress address)
        {
            _storeAddresses.Add(address);
            Touch();
        }

        public void RemoveAddress(Guid addressId)
        {
            var address = _storeAddresses.FirstOrDefault(a => a.Id == addressId);
            if (address == null) return;
            _storeAddresses.Remove(address);
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
    }
}