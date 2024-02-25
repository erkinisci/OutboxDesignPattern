using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Processor;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

//builder.Services.AddHostedService<Worker>();


builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
    {
        o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
        o.UsePostgres();
    });
    
    x.SetKebabCaseEndpointNameFormatter();

    var assembly = typeof(Program).Assembly;
    
    x.AddConsumers(assembly);
    x.AddActivities(assembly);
    
    x.UsingAzureServiceBus((ctx, cfg) =>
    {
        cfg.Host(configuration.GetConnectionString("ServiceBus"), _ => {});
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(configuration.GetConnectionString("Database"), opt =>
    {
        opt.EnableRetryOnFailure(5);
    });
});

var host = builder.Build();
host.Run();