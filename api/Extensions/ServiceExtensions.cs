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

}