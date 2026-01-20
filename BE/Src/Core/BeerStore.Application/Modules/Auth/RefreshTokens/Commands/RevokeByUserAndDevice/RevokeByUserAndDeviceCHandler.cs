using BeerStore.Application.Interface.IUnitOfWork.Auth;
using BeerStore.Application.Interface.Services.Authorization;
using BeerStore.Application.Mapping.Auth.RefreshTokenMap;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BeerStore.Application.Modules.Auth.RefreshTokens.Commands.RevokeByUserAndDevice
{
    public class RevokeByUserAndDeviceCHandler : IRequestHandler<RevokeByUserAndDeviceCommand, bool>
    {
        private readonly IAuthUnitOfWork _auow;
        private readonly ILogger<RevokeByUserAndDeviceCHandler> _logger;
        private readonly IAuthAuthorizationService _authService;

        public RevokeByUserAndDeviceCHandler(IAuthUnitOfWork auow, ILogger<RevokeByUserAndDeviceCHandler> logger, IAuthAuthorizationService authService)
        {
            _auow = auow;
            _logger = logger;
            _authService = authService;
        }

        public async Task<bool> Handle(RevokeByUserAndDeviceCommand command, CancellationToken token)
        {
            _authService.EnsureCanRevokeRefreshToken(command.UserId);

            var refreshToken = await _auow.RRefreshTokenRepository.GetByUserIdAndDeviceIdAsync(command.UserId, command.DeviceId, token);

            if (refreshToken == null)
            {
                return false;
            }

            await _auow.BeginTransactionAsync(token);

            try
            {
                refreshToken.ApplyRefreshToke(command.UpdatedBy);
                _auow.WRefreshTokenRepository.Update(refreshToken);
                await _auow.CommitTransactionAsync(token);

                return true;
            }
            catch (Exception ex)
            {
                await _auow.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Failed to revoke RefreshToken. UserId: {UserId}, DeviceId: {DeviceId}",
                    command.UserId, command.DeviceId);
                throw;
            }
        }
    }
}
