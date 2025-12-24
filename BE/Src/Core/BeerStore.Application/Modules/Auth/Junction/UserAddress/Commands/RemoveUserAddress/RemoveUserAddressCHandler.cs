using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddress.Commands.RemoveUserAddress
{
    public class RemoveUserAddressCHandler : IRequestHandler<RemoveUserAddressCommand, Unit>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RemoveUserAddressCHandler> _logger;

        public RemoveUserAddressCHandler(IAuthUnitOfWork auow, ILogger<RemoveUserAddressCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveUserAddressCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var userAddress = await _auow.RUserAddressRepository.GetByIdAsync(command.UserAddressId, token);

                if (userAddress == null)
                    throw new BusinessRuleException<UserAddressField>(
                        ErrorCategory.NotFound,
                        UserAddressField.UserId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.UserAddressId }
                        });

                _auow.WUserAddressRepository.Remove(userAddress);
                await _auow.CommitTransactionAsync(token);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while removing UserAddress. UserAddressId: {UserAddressId}",
                    command.UserAddressId
                );
                throw;
            }
        }
    }
}
