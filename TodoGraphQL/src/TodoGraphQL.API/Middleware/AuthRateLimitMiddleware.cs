using Microsoft.Extensions.Logging;
using System.Threading.RateLimiting;

namespace TodoGraphQL.API.Middleware;

public class AuthRateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthRateLimitMiddleware> _logger;
    private static readonly Dictionary<string, FixedWindowRateLimiter> _limiters = new();
    private static readonly SemaphoreSlim _lock = new(1, 1);
    private static readonly string[] AuthOperations = ["Login", "Register"];

    public AuthRateLimitMiddleware(RequestDelegate next, ILogger<AuthRateLimitMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/graphql" && context.Request.Method == "POST")
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            var isAuthOperation = AuthOperations.Any(op =>
                body.Contains($"\"operationName\":\"{op}\""));

            if (isAuthOperation)
            {
                var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var limiter = await GetOrCreateLimiter(ip);
                var lease = await limiter.AcquireAsync(1);

                if (!lease.IsAcquired)
                {
                    _logger.LogWarning(
                        "Rate limit atingido para IP: {IpAddress} | Operação: Auth",
                        ip);

                    context.Response.StatusCode = 429;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(
                        """{"errors":[{"message":"Muitas tentativas. Aguarde 1 minuto."}]}""");
                    return;
                }
            }
        }

        await _next(context);
    }

    private static async Task<FixedWindowRateLimiter> GetOrCreateLimiter(string ip)
    {
        await _lock.WaitAsync();
        try
        {
            if (!_limiters.TryGetValue(ip, out var limiter))
            {
                limiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
                _limiters[ip] = limiter;
            }
            return limiter;
        }
        finally
        {
            _lock.Release();
        }
    }
}