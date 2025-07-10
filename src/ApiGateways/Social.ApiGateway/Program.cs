using Microsoft.AspNetCore.Mvc;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// YARP'i config üzerinden yükle
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Prometheus 
app.UseHttpMetrics();
app.MapMetrics();

// Health endpoint – diðer servisleri kontrol eder
app.MapGet("/health", async ([FromServices] IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();
    try
    {
        var identity = await client.GetAsync("http://localhost:7001/health");
        var user = await client.GetAsync("http://localhost:7002/health");

        if (!identity.IsSuccessStatusCode || !user.IsSuccessStatusCode)
            return Results.StatusCode(503);

        return Results.Ok(new { status = "Healthy" });
    }
    catch
    {
        return Results.StatusCode(503);
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Manuel olarak mikroservislerin swagger endpoint'lerini ekle
        options.SwaggerEndpoint("/swagger/identity/v1/swagger.json", "Identity Service API V1");
        options.SwaggerEndpoint("/swagger/user/v1/swagger.json", "User Service API V1");
        options.RoutePrefix = string.Empty;
    });
}

//app.UseHttpsRedirection();
app.UseAuthorization();

// YARP proxy middleware aktif hale getirilir
app.MapReverseProxy();

app.MapControllers();

app.Run();