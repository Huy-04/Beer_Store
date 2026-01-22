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

namespace BeerStore.Application.Modules.Auth.UserAddresses.Queries.GetUserAddressById
{
    public class GetUserAddressByIdQHandler : IRequestHandler<GetUserAddressByIdQuery, UserAddressResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetUserAddressByIdQHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public GetUserAddressByIdQHandler(IAuthUnitOfWork auow, ILogger<GetUserAddressByIdQHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserAddressResponse> Handle(GetUserAddressByIdQuery query, CancellationToken token)
        {
            await _authService.EnsureCanReadAddress(query.IdUserAddress);

            var address = await _auow.RUserAddressRepository.GetByIdAsync(query.IdUserAddress, token);

            if (address == null)
            {
                _logger.LogWarning("UserAddress {Id} not found", query.IdUserAddress);
                throw new BusinessRuleException<UserAddressField>(
                    ErrorCategory.NotFound,
                    UserAddressField.IdAddress,
                    ErrorCode.IdNotFound,
                    new Dictionary<object, object>
                    {
                        { ParamField.Value, query.IdUserAddress }
                    });
            }

            return address.ToUserAddressResponse();
        }
    }
}
