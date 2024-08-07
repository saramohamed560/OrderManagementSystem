using OrderManagement.Apis.Errors;
using System.Net;
using System.Text.Json;

namespace OrderManagement.Apis.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next ,ILogger<ExceptionMiddleware> logger,IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }catch (Exception e)
            {
                _logger.LogError(e.Message);
                httpContext.Response.StatusCode =(int) HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType ="application/json";
                var response = _env.IsDevelopment() ?
                    new ApiExceptionRespomse((int)HttpStatusCode.InternalServerError, e.Message, e.StackTrace) :
                    new ApiExceptionRespomse((int)HttpStatusCode.InternalServerError);
                var jsonResponse = JsonSerializer.Serialize(response);
                var option = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                await httpContext.Response.WriteAsync(jsonResponse);
            }
        }
       
    }
}
