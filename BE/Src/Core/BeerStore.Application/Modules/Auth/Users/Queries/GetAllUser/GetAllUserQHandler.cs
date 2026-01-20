using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserMap;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetAllUser
{
    public class GetAllUserQHandler : IRequestHandler<GetAllUserQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly IAuthAuthorizationService _authService;

        public GetAllUserQHandler(IAuthUnitOfWork auow, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _authService = authService;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetAllUserQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllUsers();

            var list = await _auow.RUserRepository.GetAllAsync(token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}

