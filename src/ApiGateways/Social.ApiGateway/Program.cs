using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

// ?? Aspire destekli servis tan�mlar� ve health check altyap�s�
builder.AddServiceDefaults();

// ?? Reverse proxy (YARP) config + Aspire Discovery entegrasyonu
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddAspireServiceDiscovery();

builder.Services.AddControllers();
builder.Services.AddOpenApi(); // Swagger i�in (Aspire otomatik OpenAPI deste�i)

var app = builder.Build();

// ?? Swagger (iste�e ba�l� ama �nerilir)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ?? HTTPS, yetkilendirme, y�nlendirme
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ?? YARP reverse proxy routing
app.MapReverseProxy();

// ?? Aspire health, metrics ve discovery endpoint'lerini a�ar (/health, /metrics, /)
app.MapDefaultEndpoints();

app.Run();
