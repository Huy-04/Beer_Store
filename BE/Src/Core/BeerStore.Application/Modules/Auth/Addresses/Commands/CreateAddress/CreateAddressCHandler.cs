using BeerStore.Application.DTOs.Auth.Address.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.AddressMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Addresses.Commands.CreateAddress
{
    public class CreateAddressCHandler : IRequestHandler<CreateAddressCommand, AddressResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<CreateAddressCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public CreateAddressCHandler(IAuthUnitOfWork auow, ILogger<CreateAddressCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<AddressResponse> Handle(CreateAddressCommand command, CancellationToken token)
        {
            _authService.EnsureCanCreateAddress(command.UserId);

            await _auow.BeginTransactionAsync(token);

            try
            {
                var address = command.Request.ToAddress(command.UserId, command.CreatedBy, command.UpdateBy);

                await _auow.WAddressRepository.AddAsync(address, token);
                await _auow.CommitTransactionAsync(token);

                return address.ToAddressResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Failed to create Address. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}

