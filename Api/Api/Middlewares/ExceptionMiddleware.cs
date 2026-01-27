namespace Api.Middlewares;
public sealed class ExceptionMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly RequestDelegate _next = next;
    private readonly IConfiguration _configuration = configuration;
}