using Contracts;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

namespace api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services){
        services.AddCors(opions =>
        {
            opions.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });
    }

   public static void ConfigureIISIntegration(this IServiceCollection services) =>
      services.Configure<IISOptions>(options =>
    {
    });

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
    services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureSqlContext(this IServiceCollection services,
    IConfiguration configuration) =>
           services.AddDbContext<RepositoryContext>(opts =>
           opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
}