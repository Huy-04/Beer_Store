using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Shop.StoreMap;
using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.BanStore
{
    public class BanStoreCHandler : IRequestHandler<BanStoreCommand, StoreResponse>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<BanStoreCHandler> _logger;

        public BanStoreCHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<BanStoreCHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<StoreResponse> Handle(BanStoreCommand command, CancellationToken token)
        {
            _authService.EnsureCanBanStore();

            await _suow.BeginTransactionAsync(token);

            try
            {
                var store = await _suow.RStoreRepository.GetByIdAsync(command.StoreId, token);
                if (store == null)
                {
                    throw new BusinessRuleException<StoreField>(
                        ErrorCategory.NotFound,
                        StoreField.Id,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object> { { "StoreId", command.StoreId } });
                }

                store.Ban();
                store.SetUpdatedBy(command.UpdatedBy);

                _suow.WStoreRepository.Update(store);
                await _suow.CommitTransactionAsync(token);

                _logger.LogInformation("Store {StoreId} banned by {AdminId}. Reason: {Reason}", store.Id, command.UpdatedBy, command.Reason);

                return store.ToStoreResponse();
            }
            catch (Exception ex)
            {
                await _suow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to ban store {StoreId}", command.StoreId);
                throw;
            }
        }
    }
}
