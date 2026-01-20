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

namespace BeerStore.Application.Modules.Auth.Addresses.Queries.GetAddressById
{
    public class GetAddressByIdQHandler : IRequestHandler<GetAddressByIdQuery, AddressResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetAddressByIdQHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public GetAddressByIdQHandler(IAuthUnitOfWork auow, ILogger<GetAddressByIdQHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<AddressResponse> Handle(GetAddressByIdQuery query, CancellationToken token)
        {
            await _authService.EnsureCanReadAddress(query.IdAddress);

            var address = await _auow.RAddressRepository.GetByIdAsync(query.IdAddress, token);

            if (address == null)
            {
                _logger.LogWarning("Address {Id} not found", query.IdAddress);
                throw new BusinessRuleException<AddressField>(
                    ErrorCategory.NotFound,
                    AddressField.IdAddress,
                    ErrorCode.IdNotFound,
                    new Dictionary<object, object>
                    {
                        { ParamField.Value, query.IdAddress }
                    });
            }

            return address.ToAddressResponse();
        }
    }
}

