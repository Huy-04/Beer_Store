using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.Junction.UserRoleMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Queries.GetAllUserRoles
{
    public class GetAllUserRolesQHandler : IRequestHandler<GetAllUserRolesQuery, IEnumerable<UserRoleResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetAllUserRolesQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<UserRoleResponse>> Handle(GetAllUserRolesQuery query, CancellationToken token)
        {
            var userRoles = await _auow.RUserRoleRepository.GetAllAsync(token);
            return userRoles.Select(ur => ur.ToUserRoleResponse());
        }
    }
}
