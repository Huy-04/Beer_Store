using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Shop.StoreMap;
using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.RejectStore
{
    public class RejectStoreCHandler : IRequestHandler<RejectStoreCommand, StoreResponse>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<RejectStoreCHandler> _logger;

        public RejectStoreCHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<RejectStoreCHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<StoreResponse> Handle(RejectStoreCommand command, CancellationToken token)
        {
            _authService.EnsureCanRejectStore();

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

                store.Reject();
                store.SetUpdatedBy(command.UpdatedBy);

                _suow.WStoreRepository.Update(store);
                await _suow.CommitTransactionAsync(token);

                _logger.LogInformation("Store {StoreId} rejected by {AdminId}. Reason: {Reason}", store.Id, command.UpdatedBy, command.Reason);

                return store.ToStoreResponse();
            }
            catch (Exception ex)
            {
                await _suow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to reject store {StoreId}", command.StoreId);
                throw;
            }
        }
    }
}
