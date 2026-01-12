using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByPhone
{
    public class GetUserByPhoneQHandler : IRequestHandler<GetUserByPhoneQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetUserByPhoneQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByPhoneQuery query, CancellationToken token)
        {
            var list = await _auow.RUserRepository.FindAsync(u => u.Phone == query.Phone, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}
