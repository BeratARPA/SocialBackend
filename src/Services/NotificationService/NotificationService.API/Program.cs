using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Notification Service API", Version = "v1" });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddSingleton(sp =>
{
    var config = new EventBusConfig
    {
        ConnectionRetryCount = 5,
        EventBusType = EventBusType.RabbitMQ,
        SubscriberClientAppName = "NotificationService",
        DefaultTopicName = "SocialEventBus",
        Connection = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        }
    };
    return EventBusFactory.Create(config, sp);
});

builder.Services.AddAuthentication("Bearer")
  .AddJwtBearer("Bearer", options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = builder.Configuration["Jwt:Issuer"],
          ValidAudience = builder.Configuration["Jwt:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
      };
  });

builder.Services.AddAuthorization();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Prometheus 
app.UseHttpMetrics();
app.MapMetrics();

app.UseCors("AllowAll");

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(new { status = report.Status.ToString() });
        await context.Response.WriteAsync(json);
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                var serverUrl = $"{httpReq.Scheme}://{httpReq.Host.Value}";
                if (httpReq.Path.Value.Contains("/swagger/notification"))
                {
                    serverUrl += "/notification";
                }
                swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
            });
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification Service API V1");
            c.RoutePrefix = "swagger";
        });
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// EventBus subscriber:
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<PasswordResetRequestedIntegrationEvent, PasswordResetRequestedIntegrationEventHandler>();

app.Run();