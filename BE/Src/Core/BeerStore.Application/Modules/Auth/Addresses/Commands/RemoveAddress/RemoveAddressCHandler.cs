using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Addresses.Commands.RemoveAddress
{
    public class RemoveAddressCHandler : IRequestHandler<RemoveAddressCommand, bool>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveAddressCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RemoveAddressCHandler(IAuthUnitOfWork auow, ILogger<RemoveAddressCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<bool> Handle(RemoveAddressCommand command, CancellationToken token)
        {
            await _authService.EnsureCanRemoveAddress(command.IdAddress);

            await _auow.BeginTransactionAsync(token);

            try
            {
                var address = await _auow.RAddressRepository.GetByIdAsync(command.IdAddress, token);

                if (address == null)
                {
                    _logger.LogWarning("Address {Id} not found", command.IdAddress);
                    throw new BusinessRuleException<AddressField>(
                        ErrorCategory.NotFound,
                        AddressField.IdAddress,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.IdAddress }
                        });
                }

                _auow.WAddressRepository.Remove(address);
                await _auow.CommitTransactionAsync(token);
                return true;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to remove Address. Id: {Id}",
                    command.IdAddress
                );
                throw;
            }
        }
    }
}

