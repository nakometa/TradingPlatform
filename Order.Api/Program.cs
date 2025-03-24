using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Consumers;
using Order.Api.Data;
using Order.Api.Services;
using Order.Api.Services.Interfaces;
using Shared.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

// Add services to the container.

builder.Services.AddControllers();

// Add DbContext:
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(connectionString));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddRequestClient<OrderRequest>();
    busConfigurator.AddConsumer<PriceBatchUpdateConsumer>();

    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSingleton<IPriceCache, PriceCache>();
builder.Services.AddScoped<IAddOrderService, AddOrderService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
