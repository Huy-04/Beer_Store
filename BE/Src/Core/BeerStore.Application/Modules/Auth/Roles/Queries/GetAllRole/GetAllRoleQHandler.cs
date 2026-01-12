using BeerStore.Application.DTOs.Auth.Role.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.RoleMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Roles.Queries.GetAllRole
{
    public class GetAllRoleQHandler : IRequestHandler<GetAllRoleQuery, IEnumerable<RoleResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetAllRoleQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<RoleResponse>> Handle(GetAllRoleQuery query, CancellationToken token)
        {
            var list = await _auow.RRoleRepository.GetAllAsync(token);
            return list.Select(r => r.ToRoleResponse());
        }
    }
}
