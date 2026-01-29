using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserAddressMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddresses.Commands.CreateUserAddress
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
                var user = await _auow.RUserRepository.GetByIdAsync(command.UserId, token);
                // Validate user exists? Usually done by EnsureCanCreateAddress or similar, but good to check.
                if (user == null) throw new Exception("User not found"); // Should use BusinessException

                var address = command.Request.ToUserAddress(command.UserId);
                
                user.AddAddress(address);
                _auow.WUserRepository.Update(user);
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
