using BeerStore.Application.DTOs.Auth.Permission.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.PermissionMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Permissions.Queries.GetAllPermission
{
    public class GetAllPermissionQHandler : IRequestHandler<GetAllPermissionQuery, IEnumerable<PermissionResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetAllPermissionQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<PermissionResponse>> Handle(GetAllPermissionQuery query, CancellationToken token)
        {
            var list = await _auow.RPermissionRepository.GetAllAsync(token);
            return list.Select(p => p.ToPermissionResponse());
        }
    }
}
