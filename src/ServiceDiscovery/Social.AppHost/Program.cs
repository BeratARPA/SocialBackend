var builder = DistributedApplication.CreateBuilder(args);

// IdentityService
var identity = builder.AddProject<Projects.IdentityService_API>("IdentityService")
    .WithHttpHealthCheck("http://localhost:7001/health");

// UserService
var user = builder.AddProject<Projects.UserService_API>("UserService")
    .WithHttpHealthCheck("http://localhost:7002/health");

// ApiGateway
builder.AddProject<Projects.Social_ApiGateway>("ApiGateway")
       .WithReference(identity)
       .WithReference(user);

builder.Build().Run();
