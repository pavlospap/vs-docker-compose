using EshopApi;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EshopDbContext>(
    opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Eshop API",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eshop API V1");
});

app.MapPost("/api/customers", async (EshopDbContext dbContext, Customer customer) =>
{
    await dbContext.Customers.AddAsync(customer);
    await dbContext.SaveChangesAsync();

    return Results.Ok();
});

app.MapGet("/api/customers", async (EshopDbContext dbContext) =>
{
    var customers = await dbContext.Customers.ToListAsync();

    return Results.Ok(customers);
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EshopDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
