using System.Text;

namespace MOI_Eservice.MiddleWare
{
    public class TokenHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Session.TryGetValue("Token", out var tokenBytes))
            {
                var token = Encoding.UTF8.GetString(tokenBytes);
                context.Request.Headers["Authorization"] = $"Bearer {token}";
            }

            await _next(context);
        }
    }
}
