using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.User.Queries.GetUserByPhoneStatus
{
    public class GetUserByPhoneStatusQHandler : IRequestHandler<GetUserByPhoneStatusQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<GetUserByPhoneStatusQHandler> _logger;

        public GetUserByPhoneStatusQHandler(IAuthUnitOfWork auow, ILogger<GetUserByPhoneStatusQHandler> logger)
        {
            _auow = auow;
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByPhoneStatusQuery query, CancellationToken token)
        {
            var phoneStatus = PhoneStatus.Create(query.PhoneStatus);
            var list = await _auow.RUserRepository.FindAsync(u => u.PhoneStatus == phoneStatus, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}