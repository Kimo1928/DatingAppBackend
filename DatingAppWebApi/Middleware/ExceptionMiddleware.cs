using System.Text.Json;

namespace DatingAppWebApi.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger,IHostEnvironment env)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try { await next(context); }
            catch (System.Exception ex) {

                logger.LogError(ex, "{message}", ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                var response = env.IsDevelopment() ? new Errors.ApiExceptions(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) :
                    new Errors.ApiExceptions(context.Response.StatusCode, ex.Message, "Internal Server Error ");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        
        }
    }
}
