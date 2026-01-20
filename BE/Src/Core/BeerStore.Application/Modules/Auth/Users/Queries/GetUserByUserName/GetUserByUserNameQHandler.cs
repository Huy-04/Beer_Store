using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByUserName
{
    public class GetUserByUserNameQHandler : IRequestHandler<GetUserByUserNameQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetUserByUserNameQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByUserNameQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllUsers();

            var list = await _auow.RUserRepository.FindAsync(u => u.UserName == query.UserName, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}

