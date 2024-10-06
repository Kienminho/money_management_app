using Common.UnitOfWork.UnitOfWorkPattern;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.UnitOfWork
{
    public static class ServiceRegistration
    {
        public static IServiceCollection CreateDefaultDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUnitOfWork, UnitOfWorkPattern.UnitOfWork>();
            return services;
        }
    }
}