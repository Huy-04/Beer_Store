using BeerStore.Application.DTOs.Auth.Role.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.RoleMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Roles.Queries.GetAllRole
{
    public class GetAllRoleQHandler : IRequestHandler<GetAllRoleQuery, IEnumerable<RoleResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetAllRoleQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<RoleResponse>> Handle(GetAllRoleQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadRole();

            var list = await _auow.RRoleRepository.GetAllAsync(token);
            return list.Select(r => r.ToRoleResponse());
        }
    }
}

