namespace restaurantAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;
        public RequestLoggingMiddleware(RequestDelegate next ,ILogger<RequestLoggingMiddleware> logger) 
        { 
           _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            _logger.LogInformation(
                $"Request '{context.Request.Method} {context.Request.Path}'");
        }
    }
}
