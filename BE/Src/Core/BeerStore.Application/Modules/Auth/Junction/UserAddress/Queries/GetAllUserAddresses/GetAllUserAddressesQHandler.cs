using BeerStore.Application.DTOs.Auth.Junction.UserAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.Junction.UserAddressMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddress.Queries.GetAllUserAddresses
{
    public class GetAllUserAddressesQHandler : IRequestHandler<GetAllUserAddressesQuery, IEnumerable<UserAddressResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetAllUserAddressesQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<UserAddressResponse>> Handle(GetAllUserAddressesQuery query, CancellationToken token)
        {
            var userAddresses = await _auow.RUserAddressRepository.GetAllAsync(token);
            return userAddresses.Select(ua => ua.ToUserAddressResponse());
        }
    }
}
