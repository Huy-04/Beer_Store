using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserAddressMap;
using Domain.Core.ValueObjects.Address;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddresses.Queries.GetUserAddressByPhone
{
    public class GetUserAddressByPhoneQHandler : IRequestHandler<GetUserAddressByPhoneQuery, IEnumerable<UserAddressResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetUserAddressByPhoneQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserAddressResponse>> Handle(GetUserAddressByPhoneQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllAddresses();

            var phone = Phone.Create(query.Phone);
            var list = await _auow.RUserAddressRepository.FindAsync(a => a.Phone == phone, token);
            return list.Select(a => a.ToUserAddressResponse());
        }
    }
}
