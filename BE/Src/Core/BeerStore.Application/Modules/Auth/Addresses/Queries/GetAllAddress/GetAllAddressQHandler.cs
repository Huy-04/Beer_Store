using BeerStore.Application.DTOs.Auth.Address.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.AddressMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Addresses.Queries.GetAllAddress
{
    public class GetAllAddressQHandler : IRequestHandler<GetAllAddressQuery, IEnumerable<AddressResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetAllAddressQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<AddressResponse>> Handle(GetAllAddressQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllAddresses();

            var list = await _auow.RAddressRepository.GetAllAsync(token);
            return list.Select(a => a.ToAddressResponse());
        }
    }
}

