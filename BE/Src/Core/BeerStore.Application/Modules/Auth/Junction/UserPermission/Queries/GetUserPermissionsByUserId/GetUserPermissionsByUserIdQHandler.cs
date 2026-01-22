using BeerStore.Application.DTOs.Auth.Junction.UserPermission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.JunctionMap.UserPermissionMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserPermission.Queries.GetUserPermissionsByUserId
{
    public class GetUserPermissionsByUserIdQHandler : IRequestHandler<GetUserPermissionsByUserIdQuery, IEnumerable<UserPermissionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetUserPermissionsByUserIdQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserPermissionResponse>> Handle(GetUserPermissionsByUserIdQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadUserPermission();

            var list = await _auow.RUserPermissionRepository.FindAsync(up => up.UserId == query.UserId, token);
            return list.Select(up => up.ToUserPermissionResponse());
        }
    }
}
