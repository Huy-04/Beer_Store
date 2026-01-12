using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByUserName
{
    public class GetUserByUserNameQHandler : IRequestHandler<GetUserByUserNameQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetUserByUserNameQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByUserNameQuery query, CancellationToken token)
        {
            var list = await _auow.RUserRepository.FindAsync(u => u.UserName == query.UserName, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}
