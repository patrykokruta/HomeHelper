using HomeHelper.Broker.Configurations;
using HomeHelper.Client.Configurations;
using HomeHelper.CQRS;
using HomeHelper.DB;
using HomeHelper.Repositories;
using HomeHelper.Services;
using HomeHelper.Services.Plugins;
using HomeHelper.UI.ActionFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeHelper.UI
{
    public class Startup
    {
        public IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            foreach (var device in DeviceLoader.Load())
            {
                device.ConfigureServices(services);
            }
            services.AddScoped<ValidationFilterAttribute>();
            services.AddServiceLayer();
            services.AddPersistenceInfrastructure(_config);
            services.AddUnitOfWork();
            services.AddCqrs();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.ConfigureMqttService();
            services.AddSignalR();
            services.AddHostedMqttClient(_config);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
            });

            app.UseConfiguredMqttServer();
            app.UseConfiguredSignalR();
        }
    }
}
