using MediatR;

namespace IdentityService.Application.Commands.Logout
{
    public record LogoutUserCommand(string RefreshToken) : IRequest<Unit>;
}
