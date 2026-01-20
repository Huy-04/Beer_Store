using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.JunctionMap.UserRoleMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Queries.GetAllUserRoles
{
    public class GetAllUserRolesQHandler : IRequestHandler<GetAllUserRolesQuery, IEnumerable<UserRoleResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetAllUserRolesQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserRoleResponse>> Handle(GetAllUserRolesQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadUserRole();

            var userRoles = await _auow.RUserRoleRepository.GetAllAsync(token);
            return userRoles.Select(ur => ur.ToUserRoleResponse());
        }
    }
}

