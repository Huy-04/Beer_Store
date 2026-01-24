using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Shop.StoreMap;
using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.ResubmitStore
{
    public class ResubmitStoreCHandler : IRequestHandler<ResubmitStoreCommand, StoreResponse>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<ResubmitStoreCHandler> _logger;

        public ResubmitStoreCHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<ResubmitStoreCHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<StoreResponse> Handle(ResubmitStoreCommand command, CancellationToken token)
        {
            await _authService.EnsureCanResubmitStore(command.StoreId);

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

                store.ApplyResubmit(command.Request, command.UpdatedBy);

                _suow.WStoreRepository.Update(store);
                await _suow.CommitTransactionAsync(token);

                _logger.LogInformation("Store {StoreId} resubmitted by owner", store.Id);

                return store.ToStoreResponse();
            }
            catch (Exception ex)
            {
                await _suow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to resubmit store {StoreId}. Request: {@Request}", command.StoreId, command.Request);
                throw;
            }
        }
    }
}
