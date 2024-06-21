
using System.Net;
using System.Text.Json;

namespace backend.Middleware
{
    public class NotFoundMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                // Không có endpoint nào được liên kết với yêu cầu
                Console.WriteLine(endpoint);
                context.Response.StatusCode = 404;
                var response = new
                {
                    status = (int)HttpStatusCode.NotFound,
                    message = "Không tìm thấy đường dẫn yêu cầu."
                };
                var json = JsonSerializer.Serialize(response);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
                return;
            }else{
                await _next(context);
                return;
            }
        }
    }


}