using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.JunctionMap.UserRoleMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Queries.GetUserRolesByUserId
{
    public class GetUserRolesByUserIdQHandler : IRequestHandler<GetUserRolesByUserIdQuery, IEnumerable<UserRoleResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetUserRolesByUserIdQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserRoleResponse>> Handle(GetUserRolesByUserIdQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadUserRole();

            var userRoles = await _auow.RUserRoleRepository.FindAsync(ur => ur.UserId == query.UserId, token);
            return userRoles.Select(ur => ur.ToUserRoleResponse());
        }
    }
}

