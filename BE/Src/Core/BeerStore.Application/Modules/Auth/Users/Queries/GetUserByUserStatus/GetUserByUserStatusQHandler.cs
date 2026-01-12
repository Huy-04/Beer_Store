using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByUserStatus
{
    public class GetUserByUserStatusQHandler : IRequestHandler<GetUserByUserStatusQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetUserByUserStatusQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByUserStatusQuery query, CancellationToken token)
        {
            var userStatus = UserStatus.Create(query.UserStatus);
            var list = await _auow.RUserRepository.FindAsync(u => u.UserStatus == userStatus, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}
