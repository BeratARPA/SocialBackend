using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/health", async ([FromServices] IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();
    var identity = await client.GetAsync("https://api.iscsocial.com/identity/health");
    var user = await client.GetAsync("https://api.iscsocial.com/user/health");

    if (!identity.IsSuccessStatusCode || !user.IsSuccessStatusCode)
        return Results.StatusCode(503);

    return Results.Ok(new { status = "Healthy" });
});


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();