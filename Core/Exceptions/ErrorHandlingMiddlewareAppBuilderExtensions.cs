using Microsoft.AspNetCore.Builder;

namespace User.Backend.Api.Core.Exceptions
{
    public static class ErrorHandlingMiddlewareAppBuilderExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ErrorHandling>();
        }
    }
}
