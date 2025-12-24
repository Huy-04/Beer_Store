namespace Domain.Core.Enums
{
    public enum ErrorCode
    {
        // Not Found
        IdNotFound,
        RoleNotFound,
        UserNameNotFound,

        // Status
        InvalidStatus,

        // Name
        NameEmpty,
        NameTooLong,
        NameMinimum,
        NameAlreadyExists,

        // Value Objects
        // User
        PhoneAlreadyExists,
        EmailAlreadyExists,
        UserNameAlreadyExists,

        // Number
        NotZero,
        NotNegative,
        ExceedsMaximum,

        // Property
        DuplicateEntry,
        TypeMismatch,

        // Authentication
        UserNotFound,

        // Auth
        InvalidPassword,
        AccountInactive,

    }
}