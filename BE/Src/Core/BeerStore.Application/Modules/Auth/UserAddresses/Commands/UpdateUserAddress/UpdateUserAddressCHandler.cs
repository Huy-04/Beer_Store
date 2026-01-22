using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserAddressMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.UserAddresses.Commands.UpdateUserAddress
{
    public class UpdateUserAddressCHandler : IRequestHandler<UpdateUserAddressCommand, UserAddressResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<UpdateUserAddressCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public UpdateUserAddressCHandler(IAuthUnitOfWork auow, ILogger<UpdateUserAddressCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserAddressResponse> Handle(UpdateUserAddressCommand command, CancellationToken token)
        {
            await _authService.EnsureCanUpdateAddress(command.IdUserAddress);

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

                address.ApplyUserAddress(command.UpdateBy, command.Request);
                _auow.WUserAddressRepository.Update(address);
                await _auow.CommitTransactionAsync(token);

                return address.ToUserAddressResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to update UserAddress. Id: {Id}",
                    command.IdUserAddress
                );
                throw;
            }
        }
    }
}
