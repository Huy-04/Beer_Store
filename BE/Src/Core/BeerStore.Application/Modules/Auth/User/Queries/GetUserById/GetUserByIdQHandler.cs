using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.Enums.Messages;
using Domain.Core.Enums;
using Domain.Core.Enums.Messages;
using Domain.Core.RuleException;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Queries.GetUserById
{
    public class GetUserByIdQHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetUserByIdQHandler> _logger;

        public GetUserByIdQHandler(IAuthUnitOfWork auow, ILogger<GetUserByIdQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<UserResponse> Handle(GetUserByIdQuery query, CancellationToken token)
        {
            var user = await _auow.RUserRepository.GetByIdAsync(query.IdUser, token);

            if (user == null)
            {
                _logger.LogWarning("Get failed: User with Id={Id} not found", query.IdUser);
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