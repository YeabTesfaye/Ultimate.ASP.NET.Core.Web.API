using api.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using LoggerService;
using Contracts;
using Service.Contracts;
using Service;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

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
// config for  type 
builder.Services.AddControllers(config => {
config.RespectBrowserAcceptHeader = true;
config.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters()
 .AddCustomCSVFormatter()
.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);


// Add controllers
builder.Services.AddControllers()
    .AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);

var app = builder.Build();

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
