var builder = DistributedApplication.CreateBuilder(args);

// IdentityService
var identity = builder.AddProject<Projects.IdentityService_API>("IdentityService");

// UserService
var user = builder.AddProject<Projects.UserService_API>("UserService");

// ApiGateway
builder.AddProject<Projects.Social_ApiGateway>("ApiGateway")
       .WithReference(identity)
       .WithReference(user);

builder.Build().Run();
