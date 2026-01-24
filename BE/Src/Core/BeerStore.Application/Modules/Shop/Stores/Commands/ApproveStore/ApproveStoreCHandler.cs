using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Shop.StoreMap;
using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Enums;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.ApproveStore
{
    public class ApproveStoreCHandler : IRequestHandler<ApproveStoreCommand, StoreResponse>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<ApproveStoreCHandler> _logger;

        public ApproveStoreCHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<ApproveStoreCHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<StoreResponse> Handle(ApproveStoreCommand command, CancellationToken token)
        {
            _authService.EnsureCanApproveStore();

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

                store.Approve();
                store.SetUpdatedBy(command.UpdatedBy);

                _suow.WStoreRepository.Update(store);
                await _suow.CommitTransactionAsync(token);

                _logger.LogInformation("Store {StoreId} approved by {AdminId}", store.Id, command.UpdatedBy);

                return store.ToStoreResponse();
            }
            catch (Exception ex)
            {
                await _suow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to approve store {StoreId}", command.StoreId);
                throw;
            }
        }
    }
}
