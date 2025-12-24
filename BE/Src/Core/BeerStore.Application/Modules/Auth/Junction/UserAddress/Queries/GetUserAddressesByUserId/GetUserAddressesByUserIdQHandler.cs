using BeerStore.Application.DTOs.Auth.Junction.UserAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.Junction.UserAddressMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddress.Queries.GetUserAddressesByUserId
{
    public class GetUserAddressesByUserIdQHandler : IRequestHandler<GetUserAddressesByUserIdQuery, IEnumerable<UserAddressResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetUserAddressesByUserIdQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<UserAddressResponse>> Handle(GetUserAddressesByUserIdQuery query, CancellationToken token)
        {
            var userAddresses = await _auow.RUserAddressRepository.FindAsync(ua => ua.UserId == query.UserId, token);
            return userAddresses.Select(ua => ua.ToUserAddressResponse());
        }
    }
}
