namespace BeerStore.Domain.Constants.Permission
{
    public static class AuthConstant
    {
        public static class User
        {
            public const string ReadAll = "User.Read.All";
            public const string ReadSelf = "User.Read.Self";
            public const string CreateAll = "User.Create.All";
            public const string CreateSelf = "User.Create.Self"; // Implicit
            public const string UpdateAll = "User.Update.All";
            public const string UpdateSelf = "User.Update.Self";
            public const string RemoveAll = "User.Remove.All";
            public const string RemoveSelf = "User.Remove.Self"; // Implicit
        }

        public static class Role
        {
            public const string ReadAll = "Role.Read.All";
            public const string CreateAll = "Role.Create.All";
            public const string UpdateAll = "Role.Update.All";
            public const string RemoveAll = "Role.Remove.All";
        }

        public static class Permission
        {
            public const string ReadAll = "Permission.Read.All";
            public const string CreateAll = "Permission.Create.All";
            public const string UpdateAll = "Permission.Update.All";
            public const string RemoveAll = "Permission.Remove.All";
        }

        public static class Address
        {
            public const string ReadAll = "Address.Read.All";
            public const string ReadSelf = "Address.Read.Self";
            public const string CreateAll = "Address.Create.All";
            public const string CreateSelf = "Address.Create.Self";
            public const string UpdateAll = "Address.Update.All";
            public const string UpdateSelf = "Address.Update.Self";
            public const string RemoveAll = "Address.Remove.All";
            public const string RemoveSelf = "Address.Remove.Self";
        }

        public static class RefreshToken
        {
            public const string ReadAll = "RefreshToken.Read.All";
            public const string ReadSelf = "RefreshToken.Read.Self";
            public const string CreateAll = "RefreshToken.Create.All";
            public const string CreateSelf = "RefreshToken.Create.Self";
            public const string RevokeAll = "RefreshToken.Revoke.All";
            public const string RevokeSelf = "RefreshToken.Revoke.Self";
        }

        public static class UserRole
        {
            public const string ReadAll = "UserRole.Read.All";
            public const string CreateAll = "UserRole.Create.All";
            public const string RemoveAll = "UserRole.Remove.All";
        }

        public static class RolePermission
        {
            public const string ReadAll = "RolePermission.Read.All";
            public const string CreateAll = "RolePermission.Create.All";
            public const string RemoveAll = "RolePermission.Remove.All";
        }
    }
}