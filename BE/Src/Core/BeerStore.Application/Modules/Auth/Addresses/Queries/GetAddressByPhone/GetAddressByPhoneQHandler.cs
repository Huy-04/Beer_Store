using BeerStore.Application.DTOs.Auth.Address.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.AddressMap;
using BeerStore.Domain.ValueObjects.Auth.User;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Addresses.Queries.GetAddressByPhone
{
    public class GetAddressByPhoneQHandler : IRequestHandler<GetAddressByPhoneQuery, IEnumerable<AddressResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetAddressByPhoneQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<AddressResponse>> Handle(GetAddressByPhoneQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllAddresses();

            var phone = Phone.Create(query.Phone);
            var list = await _auow.RAddressRepository.FindAsync(a => a.Phone == phone, token);
            return list.Select(a => a.ToAddressResponse());
        }
    }
}

