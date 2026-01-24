namespace Application.Core.Interface.Services
{
    public interface ICurrentUserContext
    {
        Guid UserId { get; }
        string? Email { get; }
        IReadOnlyList<string> Roles { get; }
        IReadOnlyList<string> Permissions { get; }
        bool HasPermission(string permission);
        bool HasAnyPermission(params string[] permissions);
    }
}
