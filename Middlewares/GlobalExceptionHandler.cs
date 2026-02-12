using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Middlewares {
    public class GlobalExceptionHandler : IExceptionHandler {

        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env) {
            _logger = logger;
            _env = env;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {

            _logger.LogError("An unhandled exception occurred: {Message}", exception.Message);

            var problemDetail = new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Detail = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred, please try again later",
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = problemDetail.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetail);

            return true;
        }
    }
}
