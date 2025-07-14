using MediatR;

namespace IdentityService.Application.Commands.Login
{
    public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserCommandResponse>;

    public record LoginUserCommandResponse(string AccessToken, string RefreshToken);
}
