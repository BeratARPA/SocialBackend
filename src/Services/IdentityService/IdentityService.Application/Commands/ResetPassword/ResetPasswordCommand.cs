using MediatR;

namespace IdentityService.Application.Commands.ResetPassword
{
    public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<Unit>;
}
