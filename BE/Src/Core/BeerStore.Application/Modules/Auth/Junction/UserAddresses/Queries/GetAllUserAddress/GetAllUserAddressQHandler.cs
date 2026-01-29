using BeerStore.Application.DTOs.Auth.UserAddress.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserAddressMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserAddresses.Queries.GetAllUserAddress
{
    public class GetAllUserAddressQHandler : IRequestHandler<GetAllUserAddressQuery, IEnumerable<UserAddressResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetAllUserAddressQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserAddressResponse>> Handle(GetAllUserAddressQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllAddresses();

            var list = await _auow.RUserAddressRepository.GetAllAsync(token);
            return list.Select(a => a.ToUserAddressResponse());
        }
    }
}
