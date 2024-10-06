using Common.Authorization.Utils;
using Microsoft.AspNetCore.Http;

namespace Common.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var accessToken = context.Request.Query["access_token"];
            // If the request is for our hub...
            var path = context.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/applicationHub")))
            {
                // Read the token out of the query string
                token = accessToken;
                context.Request.Headers["Authorization"] = "Bearer " + accessToken;
            }
            var userId = jwtUtils.ValidateToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                //TODO: Assign User
                //context.Items["User"] = userService.GetById(userId ?? Guid.Empty);
                context.Items["User"] = userId;
            }

            await _next(context);
        }
    }
}
