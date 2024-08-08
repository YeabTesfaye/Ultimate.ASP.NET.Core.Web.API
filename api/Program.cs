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


var builder = WebApplication.CreateBuilder(args);

// Configure NLog
LoggingConfig.Configure();
builder.Logging.ClearProviders();
builder.Logging.AddNLog();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();

builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.ConfigureSqlContext(builder.Configuration);

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
//this is  used to configure the behavior of API controllers  specifically regarding model state validation. 
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});
// register action filter 
builder.Services.AddScoped<ValidationFilterAttribute>();
// config for  using different type
builder.Services.AddControllers(config => {
config.RespectBrowserAcceptHeader = true;
config.ReturnHttpNotAcceptable = true;
config.InputFormatters.Insert(0,GetJsonPatchInputFormatter());
}).AddXmlDataContractSerializerFormatters()
 .AddCustomCSVFormatter()
.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

// Add controllers
builder.Services.AddControllers()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

// include the JSON Patch formatter in
NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
.Services.BuildServiceProvider()
.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
.OfType<NewtonsoftJsonPatchInputFormatter>().First();

// register the Datashapper clas 
builder.Services.AddScoped<IDataShaper<EmployeeDto>,DataShaper<EmployeeDto>>();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();
app.UseIpRateLimiting();
// Configure exception handler middleware
app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILoggerManager>());

// Configure the HTTP request pipeline
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

app.UseAuthorization();

app.MapControllers();

app.Run();
