using Application.Core.Interface.Services;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Shop.Messages;
using static Domain.Core.Helpers.AuthorizationHelper;

namespace BeerStore.Infrastructure.Services.Shop.Authorization
{
    public class ShopAuthorizationService : IShopAuthorizationService
    {
        private readonly ICurrentUserContext _currentUser;
        private readonly IShopUnitOfWork _suow;

        public ShopAuthorizationService(ICurrentUserContext currentUser, IShopUnitOfWork suow)
        {
            _currentUser = currentUser;
            _suow = suow;
        }

        #region Store - Read

        public void EnsureCanReadAllStores()
        {
            if (_currentUser.HasPermission("Store.Read.All")) return;

            ThrowForbidden(StoreField.Id);
        }

        public async Task EnsureCanReadStore(Guid storeId)
        {
            if (_currentUser.HasPermission("Store.Read.All")) return;

            if (_currentUser.HasPermission("Store.Read.Self"))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreField.Id);
        }

        public async Task EnsureCanReadOwnStore(Guid storeId)
        {
            var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
            if (store?.OwnerId == _currentUser.UserId) return;

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanViewStore(Guid storeId)
        {
            // For now, allow viewing any store (public info)
            // Can add more logic later if needed
        }

        public void EnsureCanViewAllStores()
        {
            if (_currentUser.HasPermission("Store.Read.All")) return;

            ThrowForbidden(StoreField.Id);
        }

        #endregion Store - Read

        #region Store - Write

        public void EnsureCanCreateStore()
        {
            if (_currentUser.HasPermission("Store.Create.All")) return;
            if (_currentUser.HasPermission("Store.Create.Self")) return;

            ThrowForbidden(StoreField.Id);
        }

        public async Task EnsureCanUpdateStore(Guid storeId)
        {
            if (_currentUser.HasPermission("Store.Update.All")) return;

            if (_currentUser.HasPermission("Store.Update.Self"))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreField.Id);
        }

        public async Task EnsureCanResubmitStore(Guid storeId)
        {
            // Only owner can resubmit their rejected store
            var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
            if (store?.OwnerId == _currentUser.UserId) return;

            ThrowForbidden(StoreField.Id);
        }

        public async Task EnsureCanRemoveStore(Guid storeId)
        {
            if (_currentUser.HasPermission("Store.Delete.All")) return;

            if (_currentUser.HasPermission("Store.Delete.Self"))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreField.Id);
        }

        #endregion Store - Write

        #region Store - Admin Actions

        public void EnsureCanApproveStore()
        {
            if (_currentUser.HasPermission("Store.Approve.All")) return;

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanRejectStore()
        {
            if (_currentUser.HasPermission("Store.Reject.All")) return;

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanSuspendStore()
        {
            if (_currentUser.HasPermission("Store.Suspend.All")) return;

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanBanStore()
        {
            if (_currentUser.HasPermission("Store.Ban.All")) return;

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanReactivateStore()
        {
            if (_currentUser.HasPermission("Store.Reactivate.All")) return;

            ThrowForbidden(StoreField.Id);
        }

        #endregion Store - Admin Actions

        #region StoreAddress

        public void EnsureCanReadAllStoreAddresses()
        {
            if (_currentUser.HasPermission("StoreAddress.Read.All")) return;

            ThrowForbidden(StoreAddressField.Id);
        }

        public async Task EnsureCanReadStoreAddresses(Guid storeId)
        {
            if (_currentUser.HasPermission("StoreAddress.Read.All")) return;

            if (_currentUser.HasPermission("StoreAddress.Read.Self"))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreAddressField.Id);
        }

        public async Task EnsureCanReadStoreAddress(Guid storeAddressId)
        {
            if (_currentUser.HasPermission("StoreAddress.Read.All")) return;

            if (_currentUser.HasPermission("StoreAddress.Read.Self"))
            {
                var address = await _suow.RStoreAddressRepository.GetByIdAsync(storeAddressId);
                if (address != null)
                {
                    var store = await _suow.RStoreRepository.GetByIdAsync(address.StoreId);
                    if (store?.OwnerId == _currentUser.UserId) return;
                }
            }

            ThrowForbidden(StoreAddressField.Id);
        }

        public async Task EnsureCanCreateStoreAddress(Guid storeId)
        {
            if (_currentUser.HasPermission("StoreAddress.Create.All")) return;

            if (_currentUser.HasPermission("StoreAddress.Create.Self"))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreAddressField.Id);
        }

        public async Task EnsureCanUpdateStoreAddress(Guid storeAddressId)
        {
            if (_currentUser.HasPermission("StoreAddress.Update.All")) return;

            if (_currentUser.HasPermission("StoreAddress.Update.Self"))
            {
                var address = await _suow.RStoreAddressRepository.GetByIdAsync(storeAddressId);
                if (address != null)
                {
                    var store = await _suow.RStoreRepository.GetByIdAsync(address.StoreId);
                    if (store?.OwnerId == _currentUser.UserId) return;
                }
            }

            ThrowForbidden(StoreAddressField.Id);
        }

        public async Task EnsureCanRemoveStoreAddress(Guid storeAddressId)
        {
            if (_currentUser.HasPermission("StoreAddress.Delete.All")) return;

            if (_currentUser.HasPermission("StoreAddress.Delete.Self"))
            {
                var address = await _suow.RStoreAddressRepository.GetByIdAsync(storeAddressId);
                if (address != null)
                {
                    var store = await _suow.RStoreRepository.GetByIdAsync(address.StoreId);
                    if (store?.OwnerId == _currentUser.UserId) return;
                }
            }

            ThrowForbidden(StoreAddressField.Id);
        }

        public async Task EnsureCanManageStoreAddress(Guid storeId)
        {
            if (_currentUser.HasPermission("StoreAddress.Manage.All")) return;

            if (_currentUser.HasPermission("StoreAddress.Manage.Self") ||
                _currentUser.HasPermission("StoreAddress.Create.Self") ||
                _currentUser.HasPermission("StoreAddress.Update.Self") ||
                _currentUser.HasPermission("StoreAddress.Delete.Self"))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreAddressField.Id);
        }

        #endregion StoreAddress
    }
}
