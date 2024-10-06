using Common.Authorization;
using Common.UnitOfWork;
using DomainService.Interface;
using Infrastructure.Implements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceRegistration
{
    /// <summary>
    ///     AddTransient: Chỉ tồn tại trong lần gọi đó
    ///     AddScoped: Tồn tại trong 1 vùng scope. Vd trong 1 api gọi interface được addscope đó 2 lần cùng 1 phương thức thì sẽ trả về kết quả giống nhau
    ///     AddSingleton: Tồn tại xuyên suốt trong phiên làm việc
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.CreateDefaultDbContext(configuration).BuildServiceProvider();
        services.RegisterJwtUtils(configuration);
        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        services.AddTransient<IAuthService, AuthService>();

        return services;
    }
}