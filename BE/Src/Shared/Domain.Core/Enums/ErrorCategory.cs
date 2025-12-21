namespace Domain.Core.Enums
{
    public enum ErrorCategory
    {
        // 400
        ValidationFailed = 400,

        // 404
        NotFound = 404,

        // 409
        Conflict = 409,

        // 401
        Unauthorized = 401,

        // 403
        Forbidden = 403,

        // 500
        InternalServerError = 500
    }
}