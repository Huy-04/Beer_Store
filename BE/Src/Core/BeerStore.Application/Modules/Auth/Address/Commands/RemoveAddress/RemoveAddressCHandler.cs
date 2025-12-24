using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Address.Commands.RemoveAddress
{
    public class RemoveAddressCHandler : IRequestHandler<RemoveAddressCommand, bool>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveAddressCHandler> _logger;

        public RemoveAddressCHandler(IAuthUnitOfWork auow, ILogger<RemoveAddressCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveAddressCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var address = await _auow.RAddressRepository.GetByIdAsync(command.IdAddress, token);

                if (address == null)
                {
                    _logger.LogWarning("Remove failed: Address with Id={Id} not found", command.IdAddress);
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
                    "Exception occurred while removing Address. Id: {Id}",
                    command.IdAddress
                );
                throw;
            }
        }
    }
}
