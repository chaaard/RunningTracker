using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using RunningTracker.Application.Interfaces;
using RunningTracker.Application.Services;
using RunningTracker.Infrastructure.Data;
using RunningTracker.Infrastructure.Interfaces;
using RunningTracker.Infrastructure.Repositories;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RunningTrackerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRunningActivityService, RunningActivityService>();
builder.Services.AddScoped<IRunningActivityRepository, RunningActivityRepository>();

builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RunningTracker API", Version = "v1" });
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
          .ReadFrom.Configuration(hostingContext.Configuration)
          .WriteTo.Console());


var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (httpContext, elapsed, ex) =>
    {
        if (ex != null || httpContext.Response.StatusCode > 499)
        {
            return Serilog.Events.LogEventLevel.Error;
        }
        else if (httpContext.Response.StatusCode > 399)
        {
            return Serilog.Events.LogEventLevel.Warning;
        }
        else
        {
            return Serilog.Events.LogEventLevel.Information;
        }
    };
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
