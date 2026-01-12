namespace Application.Core.Interface.ISettings
{
    public interface IJwtSettings
    {
        public string SecretKey { get; }
        public string Issuer { get; }
        public string Audience { get; }
        public int AccessTokenExpirationMinutes { get; }
        public int RefreshTokenExpirationDays { get; }
    }
}