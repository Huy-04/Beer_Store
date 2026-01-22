using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserAddressMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.UserAddresses.Commands.CreateUserAddress
{
    public class CreateUserAddressCHandler : IRequestHandler<CreateUserAddressCommand, UserAddressResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<CreateUserAddressCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public CreateUserAddressCHandler(IAuthUnitOfWork auow, ILogger<CreateUserAddressCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserAddressResponse> Handle(CreateUserAddressCommand command, CancellationToken token)
        {
            _authService.EnsureCanCreateAddress(command.UserId);

            await _auow.BeginTransactionAsync(token);

            try
            {
                var address = command.Request.ToUserAddress(command.UserId, command.CreatedBy, command.UpdateBy);

                await _auow.WUserAddressRepository.AddAsync(address, token);
                await _auow.CommitTransactionAsync(token);

                return address.ToUserAddressResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to create UserAddress. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}
