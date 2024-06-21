
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace backend.Error.Handler
{
    public class AppExceptionHandler(ILogger<AppExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<AppExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            _logger.LogError( exception, exception.Message );
            var detail = new ProblemDetails() {
                Instance = "API",
            };
            switch (exception)
            {
                case AlreadyExistsException:
                case EmailAlreadyExistsException:
                    detail.Status = (int)HttpStatusCode.Conflict;
                    detail.Detail = exception.Message;
                    break;
                case StoreNotFoundException:
                    detail.Status = (int)HttpStatusCode.NotFound;
                    detail.Detail = exception.Message;
                    break;
                case NotFoundException:
                    detail.Status = (int)HttpStatusCode.NotFound;
                    detail.Detail = exception.Message;
                    break;
                case RoleNotFoundException:
                    detail.Status = (int)HttpStatusCode.Forbidden;
                    detail.Detail = exception.Message;
                    break;
                case InvalidFormatException:
                    detail.Status = (int)HttpStatusCode.BadRequest;
                    detail.Detail = exception.Message;
                    break;
                default:
                    detail.Status = (int)HttpStatusCode.InternalServerError;
                    detail.Detail = $"API lỗi : {exception.Message}";
                    detail.Title = "API lỗi";
                    detail.Type = "Lỗi phía máy chủ";
                    break;
            }
            var response = JsonSerializer.Serialize(detail);
            httpContext.Response.StatusCode = detail.Status.Value;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(response,cancellationToken);

            return true;
        }
    }
}