using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

// ?? Aspire destekli servis tanýmlarý ve health check altyapýsý
builder.AddServiceDefaults();

// ?? Reverse proxy (YARP) config + Aspire Discovery entegrasyonu
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddAspireServiceDiscovery();

builder.Services.AddControllers();
builder.Services.AddOpenApi(); // Swagger için (Aspire otomatik OpenAPI desteði)

var app = builder.Build();

// ?? Swagger (isteðe baðlý ama önerilir)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ?? HTTPS, yetkilendirme, yönlendirme
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ?? YARP reverse proxy routing
app.MapReverseProxy();

// ?? Aspire health, metrics ve discovery endpoint'lerini açar (/health, /metrics, /)
app.MapDefaultEndpoints();

app.Run();
