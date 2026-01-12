using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetAllUser
{
    public class GetAllUserQHandler : IRequestHandler<GetAllUserQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetAllUserQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetAllUserQuery query, CancellationToken token)
        {
            var list = await _auow.RUserRepository.GetAllAsync(token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}
