using System.Diagnostics;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using MassTransit.Metadata;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Orders.Api;
using Orders.Api.Controllers;
using Orders.Api.Data;
using Orders.Api.Repositories;
using Orders.Api.Services;
using Orders.Api.Validation;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHostedService<RecreateDatabaseHostedService<AppDbContext>>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(configuration.GetConnectionString("Database"), opt =>
    {
        opt.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        opt.MigrationsHistoryTable($"__{nameof(AppDbContext)}");
        
        opt.EnableRetryOnFailure(5);
        opt.MinBatchSize(1);
    });
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddOpenTelemetry().WithTracing(x =>
{
    x.SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService("orders-api")
            .AddTelemetrySdk()
            .AddEnvironmentVariableDetector())
        .AddSource("MassTransit")
        .AddSource(nameof(OrderController))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(o =>
        {
            var host = HostMetadataCache.IsRunningInContainer ? "jaeger" : "localhost";
            var url = $"http://{host}:6831";
            o.Endpoint = new Uri(url);
            o.ExportProcessorType = ExportProcessorType.Batch;
            o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
            {
                MaxQueueSize = 2048,
                ScheduledDelayMilliseconds = 5000,
                ExporterTimeoutMilliseconds = 30000,
                MaxExportBatchSize = 512,
            };
        });
});

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

//builder.Services.AddValidatorsFromAssemblyContaining<OrderRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<IApiMarker>();
builder.Services.AddFluentValidationAutoValidation(c =>
{
    c.DisableDataAnnotationsValidation = true;
});

builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(1);
        o.UsePostgres()
            .UseBusOutbox();
    });
    
    x.UsingAzureServiceBus((ctx, cfg) =>
    {
        cfg.Host(configuration.GetConnectionString("ServiceBus"), _ => {});
        cfg.ConfigureEndpoints(ctx);
    });
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
