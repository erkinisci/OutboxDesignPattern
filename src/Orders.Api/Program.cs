using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Api;
using Orders.Api.Data;
using Orders.Api.Repositories;
using Orders.Api.Services;
using Orders.Api.Validation;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(configuration.GetConnectionString("Database"), opt =>
    {
        opt.EnableRetryOnFailure(5);
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
        o.UsePostgres().UseBusOutbox();
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
