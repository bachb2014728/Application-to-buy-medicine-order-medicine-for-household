
using System.Net;
using System.Text.Json;

namespace backend.Error.Handler
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                // Không có endpoint nào được liên kết với yêu cầu
                context.Response.StatusCode = 404;
                var response = new
                {
                    status = (int)HttpStatusCode.NotFound,
                    message = "Không tìm thấy đường dẫn yêu cầu."
                };
                var json = JsonSerializer.Serialize(response);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
            else
            {
                await _next(context);
            }
        }

    }


}