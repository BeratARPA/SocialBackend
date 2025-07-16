using MediatR;

namespace IdentityService.Application.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<Unit>;
}
