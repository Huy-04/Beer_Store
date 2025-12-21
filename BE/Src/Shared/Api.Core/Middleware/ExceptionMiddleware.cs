using Domain.Core.Enums;
using Domain.Core.RuleException;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Core.Middleware
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ruleEx) when (ruleEx.GetType().IsGenericType && ruleEx.GetType().GetGenericTypeDefinition().Name.Contains("BusinessRuleException"))
            {
                _logger.LogWarning(ruleEx, "Business rule violation");

                var errorProperty = ruleEx.GetType().GetProperty("ErrorCategory");
                var errorCategory = (ErrorCategory?)errorProperty?.GetValue(ruleEx) ?? ErrorCategory.ValidationFailed;

                var errorDetails = new List<CustomErrorDetail>
                {
                    ToCustomErrorDetail(ruleEx)
                };

                await WriteProblemDetailsAsync(context, errorCategory, errorDetails);
            }
            catch (MultiRuleException multiEx)
            {
                _logger.LogWarning(multiEx, "Multiple business rule violations");

                var errorDetails = multiEx.Errors
                    .Select(ToCustomErrorDetail)
                    .ToList();

                await WriteProblemDetailsAsync(context, ErrorCategory.ValidationFailed, errorDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                var errorDetails = new List<CustomErrorDetail>
                {
                    new CustomErrorDetail
                    {
                        Field = "ServerError",
                        ErrorCode = "UNKNOWN_SERVER_ERROR"
                    }
                };

                await WriteProblemDetailsAsync(context, ErrorCategory.InternalServerError, errorDetails);
            }
        }

        private static CustomErrorDetail ToCustomErrorDetail(Exception ex)
        {
            var fieldProperty = ex.GetType().GetProperty("Field");
            var errorCodeProperty = ex.GetType().GetProperty("ErrorCode");
            var parametersProperty = ex.GetType().GetProperty("Parameters");

            var field = fieldProperty?.GetValue(ex)?.ToString() ?? "Unknown";
            var errorCode = errorCodeProperty?.GetValue(ex)?.ToString() ?? "UNKNOWN_ERROR";
            var parameters = parametersProperty?.GetValue(ex) as IReadOnlyDictionary<object, object>;

            Dictionary<string, object>? parameter = null;

            if (parameters != null && parameters.Count > 0)
            {
                parameter = new Dictionary<string, object>();
                foreach (var kvp in parameters)
                {
                    parameter[kvp.Key.ToString() ?? "key"] = kvp.Value;
                }
            }

            return new CustomErrorDetail
            {
                Field = field,
                ErrorCode = errorCode,
                Parameter = parameter?.Count > 0 ? parameter : null
            };
        }

        private static async Task WriteProblemDetailsAsync(
            HttpContext context,
            ErrorCategory errorCategory,
            List<CustomErrorDetail> errorDetails)
        {
            var status = (int)errorCategory;

            var problem = new CustomProblemDetails
            {
                Type = $"https://httpstatuses.com/{status}",
                Status = status,
                ErrorCategory = errorCategory.ToString(),
                Title = errorDetails.FirstOrDefault()?.ErrorCode ?? errorCategory.ToString(),
                Detail = errorDetails.Count > 0 ? null : errorCategory.ToString(),
                Instance = context.Request.Path,
                TraceId = context.TraceIdentifier,
                Errors = errorDetails
            };

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/problem+json";

            var opts = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, opts));
        }
    }
}