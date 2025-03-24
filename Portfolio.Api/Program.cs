using MassTransit;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Consumers;
using Portfolio.Api.Data;
using Portfolio.Api.Services;
using Portfolio.Api.Services.Interfaces;
using Shared.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<PortfolioDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<PriceBatchUpdateConsumer>();
    busConfigurator.AddConsumer<OrderRequestConsumer>();

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
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
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
