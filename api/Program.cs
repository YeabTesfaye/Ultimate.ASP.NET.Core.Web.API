using api.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using NLog.Extensions.Logging;
using LoggerService;
using Contracts;
using Service.Contracts;
using Service;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
LoggingConfig.Configure();
builder.Logging.ClearProviders();
builder.Logging.AddNLog();
builder.Services.ConfigureSqlContext(builder.Configuration);
// this is coz the controllers are defined in  a separate class library
builder.Services.AddControllers()
.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();
app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILoggerManager>());

// app.Use(async (context, next) =>
// {
//     var logger = context.RequestServices.GetRequiredService<ILoggerManager>();
//     context.RequestServices.GetRequiredService<ILoggerManager>();
//     app.ConfigureExceptionHandler(logger);
//     await next();
// });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

// Map custom routes and middleware
app.Map("/usingmapbranch", builder =>
{
    builder.Use(async (context, next) =>
    {
        Console.WriteLine("Map branch logic in the Use method before the next delegate");
        await next.Invoke();
        Console.WriteLine("Map branch logic in the Use method after the next delegate");
    });

    builder.Run(async context =>
    {
        Console.WriteLine("Map branch response to the client in the Run method");
        context.Response.StatusCode = 300;
        await context.Response.WriteAsync("Hello from the map branch.");
    });
});

app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder =>
{
    builder.Run(async context =>
    {
        await context.Response.WriteAsync("Hello from the MapWhen branch.");
    });
});

// app.Run(async context =>
// {
//     Console.WriteLine("Writing the response to the client in the Run method");
//     context.Response.StatusCode = 200;
//     await context.Response.WriteAsync("Hello from the middleware component.");
// });
// configure routing to controllers for attributing routing
app.MapControllers();

app.Run();
