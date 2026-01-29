using Application.Core.Interface.Services;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Shop.Messages;
using static Domain.Core.Helpers.AuthorizationHelper;
using BeerStore.Domain.Constants.Permission;

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

        #region Store

        public void EnsureCanReadAllStores()
        {
            if (_currentUser.HasPermission(ShopConstant.Store.ReadAll)) return;

            ThrowForbidden(StoreField.Id);
        }

        public async Task EnsureCanReadStore(Guid storeId)
        {
            if (_currentUser.HasPermission(ShopConstant.Store.ReadAll)) return;

            if (_currentUser.HasPermission(ShopConstant.Store.ReadSelf))
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
        }

        public void EnsureCanViewAllStores()
        {
            if (_currentUser.HasPermission(ShopConstant.Store.ReadAll)) return;

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanCreateStore()
        {
            if (_currentUser.HasPermission(ShopConstant.Store.CreateAll)) return;
            if (_currentUser.HasPermission(ShopConstant.Store.CreateSelf)) return;

            ThrowForbidden(StoreField.Id);
        }

        public async Task EnsureCanUpdateStore(Guid storeId)
        {
            if (_currentUser.HasPermission(ShopConstant.Store.UpdateAll)) return;

            if (_currentUser.HasPermission(ShopConstant.Store.UpdateSelf))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreField.Id);
        }

        public async Task EnsureCanResubmitStore(Guid storeId)
        {
            var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
            if (store?.OwnerId == _currentUser.UserId) return;

            ThrowForbidden(StoreField.Id);
        }

        public async Task EnsureCanRemoveStore(Guid storeId)
        {
            if (_currentUser.HasPermission(ShopConstant.Store.DeleteAll)) return;

            if (_currentUser.HasPermission(ShopConstant.Store.DeleteSelf))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanApproveStore()
        {
            if (_currentUser.HasPermission(ShopConstant.Store.ApproveAll)) return;

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanRejectStore()
        {
            if (_currentUser.HasPermission(ShopConstant.Store.RejectAll)) return;

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanSuspendStore()
        {
            if (_currentUser.HasPermission(ShopConstant.Store.SuspendAll)) return;

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanBanStore()
        {
            if (_currentUser.HasPermission(ShopConstant.Store.BanAll)) return;

            ThrowForbidden(StoreField.Id);
        }

        public void EnsureCanReactivateStore()
        {
            if (_currentUser.HasPermission(ShopConstant.Store.ReactivateAll)) return;

            ThrowForbidden(StoreField.Id);
        }

        #endregion Store

        #region StoreAddress

        public void EnsureCanReadAllStoreAddresses()
        {
            if (_currentUser.HasPermission(ShopConstant.StoreAddress.ReadAll)) return;

            ThrowForbidden(StoreAddressField.Id);
        }

        public async Task EnsureCanReadStoreAddresses(Guid storeId)
        {
            if (_currentUser.HasPermission(ShopConstant.StoreAddress.ReadAll)) return;

            if (_currentUser.HasPermission(ShopConstant.StoreAddress.ReadSelf))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreAddressField.Id);
        }

        public async Task EnsureCanReadStoreAddress(Guid storeAddressId)
        {
            if (_currentUser.HasPermission(ShopConstant.StoreAddress.ReadAll)) return;

            if (_currentUser.HasPermission(ShopConstant.StoreAddress.ReadSelf))
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
            if (_currentUser.HasPermission(ShopConstant.StoreAddress.CreateAll)) return;

            if (_currentUser.HasPermission(ShopConstant.StoreAddress.CreateSelf))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreAddressField.Id);
        }

        public async Task EnsureCanUpdateStoreAddress(Guid storeAddressId)
        {
            if (_currentUser.HasPermission(ShopConstant.StoreAddress.UpdateAll)) return;

            if (_currentUser.HasPermission(ShopConstant.StoreAddress.UpdateSelf))
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
            if (_currentUser.HasPermission(ShopConstant.StoreAddress.DeleteAll)) return;

            if (_currentUser.HasPermission(ShopConstant.StoreAddress.DeleteSelf))
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
            if (_currentUser.HasPermission(ShopConstant.StoreAddress.ManageAll)) return;

            if (_currentUser.HasPermission(ShopConstant.StoreAddress.ManageSelf) ||
                _currentUser.HasPermission(ShopConstant.StoreAddress.CreateSelf) ||
                _currentUser.HasPermission(ShopConstant.StoreAddress.UpdateSelf) ||
                _currentUser.HasPermission(ShopConstant.StoreAddress.DeleteSelf))
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(storeId);
                if (store?.OwnerId == _currentUser.UserId) return;
            }

            ThrowForbidden(StoreAddressField.Id);
        }

        #endregion StoreAddress
    }
}
