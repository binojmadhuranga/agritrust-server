using AgriTrust.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace AgriTrust.API.Middlewares;

    public class ExceptionMiddleware
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

            catch (AuthException ex)
            {
                await HandleException(context, ex.StatusCode, ex.Message);
                _logger.LogWarning(ex, "Authentication exception occurred");
            }

            catch (ForbiddenException ex)
            {
                await HandleException(context, ex.StatusCode, ex.Message);
                _logger.LogWarning(ex, "Forbidden exception occurred");
            }

            catch (NotFoundException ex)
            {
                await HandleException(context, ex.StatusCode, ex.Message);
                _logger.LogWarning(ex, "Resource not found");
            }

            catch (ConflictException ex)
            {
                await HandleException(context, ex.StatusCode, ex.Message);
                _logger.LogWarning(ex, "Conflict occurred");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleException(
                    context,
                    (int)HttpStatusCode.InternalServerError,
                    "Something went wrong"
                );
            }
        }

        private static async Task HandleException(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
