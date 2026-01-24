namespace BeerStore.Application.Interface.Services.Authorization
{
    public interface IShopAuthorizationService
    {
        // Store - Read
        void EnsureCanReadAllStores();
        Task EnsureCanReadStore(Guid storeId);
        Task EnsureCanReadOwnStore(Guid storeId);

        // Store - Write
        void EnsureCanCreateStore();
        Task EnsureCanUpdateStore(Guid storeId);
        Task EnsureCanResubmitStore(Guid storeId);

        // Store - Admin Actions
        void EnsureCanApproveStore();
        void EnsureCanRejectStore();
        void EnsureCanSuspendStore();
        void EnsureCanBanStore();
        void EnsureCanReactivateStore();

        // StoreAddress
        Task EnsureCanReadStoreAddresses(Guid storeId);
        Task EnsureCanCreateStoreAddress(Guid storeId);
        Task EnsureCanUpdateStoreAddress(Guid storeAddressId);
        Task EnsureCanRemoveStoreAddress(Guid storeAddressId);

        // Unified methods for manage operations
        Task EnsureCanManageStoreAddress(Guid storeId);
        void EnsureCanViewStore(Guid storeId);
        void EnsureCanViewAllStores();
    }
}
