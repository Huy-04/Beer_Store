using BeerStore.Application.DTOs.Auth.RefreshToken.Requests;
using BeerStore.Domain.Entities.Auth;
using BeerStore.Domain.ValueObjects.Auth.RefreshToken;
using Domain.Core.ValueObjects.Common;
using Domain.Core.Enums;

namespace BeerStore.Application.Mapping.Auth.RefreshTokenMap
{
    public static class RequestToRefreshToken
    {
        public static RefreshToken ToRefreshToken(this RefreshTokenRequest request, Guid createdBy, Guid updatedBy, TokenHash tokenHash, DateTimeOffset expiresAt)
        {
            return RefreshToken.Create(
                request.userId,
                tokenHash,
                DeviceId.Create(request.deviceId),
                DeviceName.Create(request.deviceName),
                IpAddress.Create(request.ipAddress),
                expiresAt,
                createdBy,
                updatedBy);
        }


    }
}