using BeerStore.Application.DTOs.Shop.Store.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Shop;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Shop.StoreMap;
using BeerStore.Domain.Enums.Shop.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Shop.Stores.Commands.RegisterStore
{
    public class RegisterStoreCHandler : IRequestHandler<RegisterStoreCommand, StoreResponse>
    {
        private readonly IShopUnitOfWork _suow;
        private readonly IShopAuthorizationService _authService;
        private readonly ILogger<RegisterStoreCHandler> _logger;

        public RegisterStoreCHandler(
            IShopUnitOfWork suow,
            IShopAuthorizationService authService,
            ILogger<RegisterStoreCHandler> logger)
        {
            _suow = suow;
            _authService = authService;
            _logger = logger;
        }

        public async Task<StoreResponse> Handle(RegisterStoreCommand command, CancellationToken token)
        {
            _authService.EnsureCanCreateStore();

            await _suow.BeginTransactionAsync(token);

            try
            {
                // Check if user already has a store (1 user = 1 store for MVP)
                var existingStore = await _suow.RStoreRepository.GetByOwnerIdAsync(command.CreatedBy, token);
                if (existingStore != null)
                {
                    _logger.LogWarning("User {UserId} already has a store", command.CreatedBy);
                    throw new BusinessRuleException<StoreField>(
                        ErrorCategory.Conflict,
                        StoreField.OwnerId,
                        ErrorCode.DuplicateEntry,
                        new Dictionary<object, object> { { ParamField.Value, command.CreatedBy } });
                }

                // Check Slug unique
                if (await _suow.RStoreRepository.ExistsBySlugAsync(command.Request.Slug, token))
                {
                    _logger.LogWarning("Store with Slug {Slug} already exists", command.Request.Slug);
                    throw new BusinessRuleException<StoreField>(
                        ErrorCategory.Conflict,
                        StoreField.Slug,
                        ErrorCode.NameAlreadyExists,
                        new Dictionary<object, object> { { ParamField.Value, command.Request.Slug } });
                }

                var store = command.Request.ToStore(command.CreatedBy, command.CreatedBy, command.UpdatedBy);

                await _suow.WStoreRepository.AddAsync(store, token);
                await _suow.CommitTransactionAsync(token);

                _logger.LogInformation("Store {StoreId} registered successfully for user {UserId}", store.Id, command.CreatedBy);

                return store.ToStoreResponse();
            }
            catch (Exception ex)
            {
                await _suow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to register store. Request: {@Request}", command.Request);
                throw;
            }
        }
    }
}
