using BeerStore.Domain.ValueObjects.Auth.RefreshToken;
using Domain.Core.Enums;

namespace BeerStore.Domain.Entities.Auth
{
    public class RefreshToken : AggregateRoot
    {
        public Guid UserId { get; private set; }

        public TokenHash TokenHash { get; private set; }

        public DeviceId DeviceId { get; private set; }

        public DeviceName DeviceName { get; private set; }

        public IpAddress IpAddress { get; private set; }

        public DateTimeOffset ExpiresAt { get; private set; }

        public TokenStatus TokenStatus { get; private set; }

        public Guid CreatedBy { get; private set; }

        public Guid UpdatedBy { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        private RefreshToken()
        {
        }

        private RefreshToken(Guid id, Guid userId, TokenHash tokenHash, DeviceId deviceId, DeviceName deviceName, IpAddress ipAddress, DateTimeOffset expiresAt, TokenStatus tokenStatus, Guid createdBy, Guid updatedBy)
            : base(id)
        {
            UserId = userId;
            TokenHash = tokenHash;
            DeviceId = deviceId;
            DeviceName = deviceName;
            IpAddress = ipAddress;
            ExpiresAt = expiresAt;
            TokenStatus = tokenStatus;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            CreatedAt = UpdatedAt = DateTimeOffset.UtcNow;
        }

        public static RefreshToken Create(Guid userId, TokenHash tokenHash, DeviceId deviceId, DeviceName deviceName, IpAddress ipAddress, DateTimeOffset expiresAt, Guid createdBy, Guid updatedBy)
        {
            var refreshToken = new RefreshToken(Guid.NewGuid(), userId, tokenHash, deviceId, deviceName, ipAddress, expiresAt, TokenStatus.Active, createdBy, updatedBy);
            return refreshToken;
        }

        public void Revoke()
        {
            if (TokenStatus.Value == StatusEnum.Inactive) return;
            TokenStatus = TokenStatus.Inactive;
            Touch();
        }

        public void SetUpdatedBy(Guid updatedBy)
        {
            if (UpdatedBy == updatedBy) return;
            UpdatedBy = updatedBy;
            Touch();
        }

        public void Touch()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}