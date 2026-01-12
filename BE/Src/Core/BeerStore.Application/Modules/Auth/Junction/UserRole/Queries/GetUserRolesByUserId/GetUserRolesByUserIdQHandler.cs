using BeerStore.Application.DTOs.Auth.Junction.UserRole.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.JunctionMap.UserRoleMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Junction.UserRole.Queries.GetUserRolesByUserId
{
    public class GetUserRolesByUserIdQHandler : IRequestHandler<GetUserRolesByUserIdQuery, IEnumerable<UserRoleResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetUserRolesByUserIdQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<UserRoleResponse>> Handle(GetUserRolesByUserIdQuery query, CancellationToken token)
        {
            var userRoles = await _auow.RUserRoleRepository.FindAsync(ur => ur.UserId == query.UserId, token);
            return userRoles.Select(ur => ur.ToUserRoleResponse());
        }
    }
}
