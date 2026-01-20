using BeerStore.Application.DTOs.Auth.Address.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.AddressMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCHandler : IRequestHandler<UpdateAddressCommand, AddressResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<UpdateAddressCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public UpdateAddressCHandler(IAuthUnitOfWork auow, ILogger<UpdateAddressCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<AddressResponse> Handle(UpdateAddressCommand command, CancellationToken token)
        {
            await _authService.EnsureCanUpdateAddress(command.IdAddress);

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

                address.ApplyAddress(command.UpdateBy, command.Request);
                _auow.WAddressRepository.Update(address);
                await _auow.CommitTransactionAsync(token);

                return address.ToAddressResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to update Address. Id: {Id}",
                    command.IdAddress
                );
                throw;
            }
        }
    }
}

