var builder = DistributedApplication.CreateBuilder(args);

// IdentityService
var identity = builder.AddProject("identity", "../Services/IdentityService/IdentityService.API/IdentityService.API.csproj")
                      .WithHttpEndpoint(port: 7001);

// UserService
var user = builder.AddProject("user", "../Services/UserService/UserService.API/UserService.API.csproj")
                  .WithHttpEndpoint(port: 7002);

// Gateway
builder.AddProject("apigateway", "../ApiGateways/Social.ApiGateway/Social.ApiGateway.csproj")
       .WithHttpEndpoint(port: 8080)
       .WithReference(identity)
       .WithReference(user);

builder.Build().Run();
