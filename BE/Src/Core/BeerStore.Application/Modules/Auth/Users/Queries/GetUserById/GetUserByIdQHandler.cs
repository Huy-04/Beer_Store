using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserById
{
    public class GetUserByIdQHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetUserByIdQHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public GetUserByIdQHandler(IAuthUnitOfWork auow, ILogger<GetUserByIdQHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<UserResponse> Handle(GetUserByIdQuery query, CancellationToken token)
        {
            _authService.EnsureCanReadUser(query.IdUser);

            var user = await _auow.RUserRepository.GetByIdAsync(query.IdUser, token);

            if (user == null)
            {
                _logger.LogWarning("User {Id} not found", query.IdUser);
                throw new BusinessRuleException<UserField>(
                    ErrorCategory.NotFound,
                    UserField.IdUser,
                    ErrorCode.IdNotFound,
                    new Dictionary<object, object>
                    {
                            {ParamField.Value, query.IdUser },
                    });
            }

            return user.ToUserResponse();
        }
    }
}
