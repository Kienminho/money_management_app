using Common.Authorization.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Authorization
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterJwtUtils(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtUtils, JwtUtils>();
            return services;
        }
    }
}
