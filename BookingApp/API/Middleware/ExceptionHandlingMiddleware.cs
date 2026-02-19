using Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            object responsePayload;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    responsePayload = new { errors = validationException.Errors };
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    responsePayload = new { error = notFoundException.Message };
                    break;

                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    responsePayload = new { error = badRequestException.Message };
                    break;

                case UnauthorizedException unauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    responsePayload = new { error = unauthorizedException.Message };
                    break;

                case ForbiddenAccessException forbiddenAccessException:
                    statusCode = HttpStatusCode.Forbidden;
                    responsePayload = new { error = forbiddenAccessException.Message };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    responsePayload = new { error = "An internal server error has occurred." };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var jsonResponse = JsonSerializer.Serialize(responsePayload);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
   