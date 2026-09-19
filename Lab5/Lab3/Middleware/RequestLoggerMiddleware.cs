namespace Lab3.Middleware
{
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("[RequestLoggerMiddleware] Before calling next()");
            Console.WriteLine($"Path: {context.Request.Path} | Method: {context.Request.Method} | Time: {DateTime.Now}");

            await _next(context);

            Console.WriteLine("[RequestLoggerMiddleware] After calling next()");
        }
    }
}