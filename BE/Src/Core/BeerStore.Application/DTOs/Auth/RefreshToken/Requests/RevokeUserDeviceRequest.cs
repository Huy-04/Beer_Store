namespace BeerStore.Application.DTOs.Auth.RefreshToken.Requests
{
    public record RevokeUserDeviceRequest(Guid UserId, string DeviceId)
    {
    }
}