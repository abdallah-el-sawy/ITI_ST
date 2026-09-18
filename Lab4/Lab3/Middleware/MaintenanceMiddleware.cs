namespace Lab3.Middleware
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public MaintenanceMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            bool isMaintenanceMode = _configuration.GetValue<bool>("MaintenanceMode");

            if (isMaintenanceMode)
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("The system is currently under maintenance.");
                return; // short-circuit: the request goes no further
            }

            Console.WriteLine("[MaintenanceMiddleware] Before calling next()");
            await _next(context);
            Console.WriteLine("[MaintenanceMiddleware] After calling next()");
        }
    }
}