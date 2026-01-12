using BeerStore.Application.DTOs.Auth.User.Responses;
using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Mapping.Auth.UserMap;
using BeerStore.Domain.ValueObjects.Auth.User.Status;
using MediatR;

namespace BeerStore.Application.Modules.Auth.Users.Queries.GetUserByPhoneStatus
{
    public class GetUserByPhoneStatusQHandler : IRequestHandler<GetUserByPhoneStatusQuery, IEnumerable<UserResponse>>
    {
        private readonly IAuthUnitOfWork _auow;

        public GetUserByPhoneStatusQHandler(IAuthUnitOfWork auow)
        {
            _auow = auow;
        }

        public async Task<IEnumerable<UserResponse>> Handle(GetUserByPhoneStatusQuery query, CancellationToken token)
        {
            var phoneStatus = PhoneStatus.Create(query.PhoneStatus);
            var list = await _auow.RUserRepository.FindAsync(u => u.PhoneStatus == phoneStatus, token);
            return list.Select(u => u.ToUserResponse());
        }
    }
}
