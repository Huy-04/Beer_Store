using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByEmail
{
    public class GetUserByEmailQHandler : IRequestHandler<GetUserByEmailQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetUserByEmailQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllUsers();

            var list = await _auow.RUserRepository.FindAsync(u => u.Email == query.Email);
            return list.Select(u => u.ToUserResponse());
        }
    }
}

