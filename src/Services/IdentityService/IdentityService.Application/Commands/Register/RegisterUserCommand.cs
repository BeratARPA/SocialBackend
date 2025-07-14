using MediatR;

namespace IdentityService.Application.Commands.Register
{
    public record RegisterUserCommand(string Email, string Password) : IRequest<RegisterUserCommandResponse>;

    public record RegisterUserCommandResponse(string UserId, string Email, string Token);
}
