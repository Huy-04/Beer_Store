using BeerStore.Application.DTOs.Auth.Address.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.AddressMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Address.Commands.CreateAddress
{
    public class CreateAddressCHandler : IRequestHandler<CreateAddressCommand, AddressResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<CreateAddressCHandler> _logger;

        public CreateAddressCHandler(IAuthUnitOfWork auow, ILogger<CreateAddressCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<AddressResponse> Handle(CreateAddressCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var address = command.Request.ToAddress(command.CreatedBy, command.UpdateBy);

                await _auow.WAddressRepository.AddAsync(address, token);
                await _auow.CommitTransactionAsync(token);

                return address.ToAddressResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while creating Address. Request: {@Request}",
                    command.Request
                );
                throw;
            }
        }
    }
}
