using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetByUserByEmailStatus
{
    public class GetUserByEmailStatusQHandler : IRequestHandler<GetUserByEmailStatusQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetUserByEmailStatusQHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public GetUserByEmailStatusQHandler(IAuthUnitOfWork auow, ILogger<GetUserByEmailStatusQHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByEmailStatusQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadAllUsers();

            try
            {
                var emailStatus = EmailStatus.Create(query.EmailStatus);
                var list = await _auow.RUserRepository.FindAsync(u => u.EmailStatus == emailStatus, token);
                return list.Select(u => u.ToUserResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get users by EmailStatus {EmailStatus}", query.EmailStatus);
                throw;
            }
        }
    }
}

