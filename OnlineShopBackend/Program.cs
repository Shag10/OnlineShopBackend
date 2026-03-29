using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Preserve PascalCase property names (disable camel-casing) so existing front-end bindings continue to work
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<OnlineShopBackend.Validators.InventoryValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core DbContext with connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       "Server=(localdb)\\MSSQLLocalDB;Database=OnlineShoppingDB;TrustServerCertificate=True;";
builder.Services.AddDbContext<OnlineShopBackend.Data.AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register repository and service
builder.Services.AddScoped<OnlineShopBackend.Repositories.IInventoryRepository, OnlineShopBackend.Repositories.InventoryRepository>();
builder.Services.AddScoped<OnlineShopBackend.Services.IInventoryService, OnlineShopBackend.Services.InventoryService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Ensure DB is created and seed data for development
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<OnlineShopBackend.Data.AppDbContext>();
        db.Database.EnsureCreated();
        OnlineShopBackend.Data.DbSeeder.Seed(db);
    }
}

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
