using Microsoft.Extensions.DependencyInjection;

namespace HomeHelper.Repositories
{
    public static class ServiceRegistration
    {
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
