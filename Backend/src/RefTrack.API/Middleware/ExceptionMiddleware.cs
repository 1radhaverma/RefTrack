using System.Text.Json;

namespace RefTrack.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
            => _next = next;

        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (FluentValidation.ValidationException ex)
            {
                // ex.Errors is IEnumerable<ValidationFailure>
                // .Errors exists ONLY on FluentValidation version
                var errors = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                ctx.Response.StatusCode = 400;
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(
                    JsonSerializer.Serialize(new
                    {
                        status = 400,
                        errors
                    }));
            }
            catch (KeyNotFoundException ex)
            {
                ctx.Response.StatusCode = 404;
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(
                    JsonSerializer.Serialize(new
                    {
                        status = 404,
                        message = ex.Message
                    }));
            }
            catch (Exception ex)
            {
                ctx.Response.StatusCode = 500;
                ctx.Response.ContentType = "application/json";
                await ctx.Response.WriteAsync(
                    JsonSerializer.Serialize(new
                    {
                        status = 500,
                        message = "An unexpected error occurred"
                    }));
            }

        }
    }
}