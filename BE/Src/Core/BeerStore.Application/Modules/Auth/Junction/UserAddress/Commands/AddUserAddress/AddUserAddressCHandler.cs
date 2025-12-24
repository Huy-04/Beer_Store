using BeerStore.Application.DTOs.Auth.Junction.UserAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.Junction.UserAddressMap;
using BeerStore.Domain.Entities.Auth.Junction;
using BeerStore.Domain.Enums.Messages;
using UserAddressEntity = BeerStore.Domain.Entities.Auth.Junction.UserAddress;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddress.Commands.AddUserAddress
{
    public class AddUserAddressCHandler : IRequestHandler<AddUserAddressCommand, UserAddressResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<AddUserAddressCHandler> _logger;

        public AddUserAddressCHandler(IAuthUnitOfWork auow, ILogger<AddUserAddressCHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<UserAddressResponse> Handle(AddUserAddressCommand command, CancellationToken token)
        {
            await _auow.BeginTransactionAsync(token);

            try
            {
                var user = await _auow.RUserRepository.GetByIdAsync(command.UserId, token);
                if (user == null)
                    throw new BusinessRuleException<UserAddressField>(
                        ErrorCategory.NotFound,
                        UserAddressField.UserId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.UserId }
                        });

                var address = await _auow.RAddressRepository.GetByIdAsync(command.AddressId, token);
                if (address == null)
                    throw new BusinessRuleException<UserAddressField>(
                        ErrorCategory.NotFound,
                        UserAddressField.AddressId,
                        ErrorCode.IdNotFound,
                        new Dictionary<object, object>
                        {
                            { ParamField.Value, command.AddressId }
                        });

                var userAddress = UserAddressEntity.Create(command.UserId, command.AddressId);

                await _auow.WUserAddressRepository.AddAsync(userAddress, token);
                await _auow.CommitTransactionAsync(token);

                return userAddress.ToUserAddressResponse();
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex,
                    "Exception occurred while adding UserAddress. UserId: {UserId}, AddressId: {AddressId}",
                    command.UserId, command.AddressId
                );
                throw;
            }
        }
    }
}
