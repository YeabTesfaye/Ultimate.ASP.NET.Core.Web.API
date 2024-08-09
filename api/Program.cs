using api.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using LoggerService;
using Contracts;
using Service.Contracts;
using Service;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using CompanyEmployees.Presentation.ActionFilters;
using Shared.DataTransferObjects;
using Service.DataShaping;
using AspNetCoreRateLimit;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure NLog
LoggingConfig.Configure();
builder.Logging.ClearProviders();
builder.Logging.AddNLog();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.ConfigureSqlContext(builder.Configuration);

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure API behavior options
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Register action filter 
builder.Services.AddScoped<ValidationFilterAttribute>();

// Config for different input formatters
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
})
.AddXmlDataContractSerializerFormatters()
.AddCustomCSVFormatter()
.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

// Register the DataShaper class 
builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

var app = builder.Build();

app.UseIpRateLimiting();

// Authentication middleware to the application’s pipeline 
app.UseAuthentication();
app.UseAuthorization();

// Configure exception handler middleware
app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILoggerManager>());

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API v1");
    });
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

app.MapControllers();

app.Run();

// Include the JSON Patch formatter
NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
    .OfType<NewtonsoftJsonPatchInputFormatter>().First();
