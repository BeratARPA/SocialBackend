using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Aspire Discovery
builder.AddServiceDefaults(); // Burasý önemli

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        name: "MSSQL",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "sql" }
    );

var app = builder.Build();

// Aspire Discovery
app.MapDefaultEndpoints(); // bu discovery + metrics + health otomatik

app.MapHealthChecks("/health");

app.MapGet("/", () => "Identity Service Running!");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
