using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByEmail
{
    public class GetUserByEmailQHandler : IRequestHandler<GetUserByEmailQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetUserByEmailQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken token)
        {
            var list = await _auow.RUserRepository.FindAsync(u => u.Email == query.Email);
            return list.Select(u => u.ToUserResponse());
        }
    }
}
