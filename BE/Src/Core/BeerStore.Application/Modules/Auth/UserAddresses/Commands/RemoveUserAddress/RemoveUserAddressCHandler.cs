using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.UserAddresses.Commands.RemoveUserAddress
{
    public class RemoveUserAddressCHandler : IRequestHandler<RemoveUserAddressCommand>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveUserAddressCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RemoveUserAddressCHandler(IAuthUnitOfWork auow, ILogger<RemoveUserAddressCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task Handle(RemoveUserAddressCommand command, CancellationToken token)
        {
            await _authService.EnsureCanRemoveAddress(command.IdUserAddress);

            await _auow.BeginTransactionAsync(token);

            try
            {
                var address = await _auow.RUserAddressRepository.GetByIdAsync(command.IdUserAddress, token);

                if (address == null)
                {
                    _logger.LogWarning("UserAddress {Id} not found", command.IdUserAddress);
                    throw new BusinessRuleException<UserAddressField>(
                        ErrorCategory.NotFound,
                        UserAddressField.IdAddress,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.IdUserAddress }
                        });
                }

                _auow.WUserAddressRepository.Remove(address);
                await _auow.CommitTransactionAsync(token);
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to remove UserAddress. Id: {Id}",
                    command.IdUserAddress
                );
                throw;
            }
        }
    }
}
