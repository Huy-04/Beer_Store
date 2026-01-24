using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Shop.StoreMap;
using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.ReactivateStore
{
    public class ReactivateStoreCHandler : IRequestHandler<ReactivateStoreCommand, StoreResponse>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<ReactivateStoreCHandler> _logger;

        public ReactivateStoreCHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<ReactivateStoreCHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<StoreResponse> Handle(ReactivateStoreCommand command, CancellationToken token)
        {
            _authService.EnsureCanReactivateStore();

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

                store.Reactivate();
                store.SetUpdatedBy(command.UpdatedBy);

                _suow.WStoreRepository.Update(store);
                await _suow.CommitTransactionAsync(token);

                _logger.LogInformation("Store {StoreId} reactivated by {AdminId}", store.Id, command.UpdatedBy);

                return store.ToStoreResponse();
            }
            catch (Exception ex)
            {
                await _suow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to reactivate store {StoreId}", command.StoreId);
                throw;
            }
        }
    }
}
