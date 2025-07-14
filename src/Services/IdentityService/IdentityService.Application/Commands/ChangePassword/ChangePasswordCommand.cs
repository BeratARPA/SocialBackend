using MediatR;

namespace IdentityService.Application.Commands.ChangePassword
{
    public record ChangePasswordCommand(Guid UserId, string OldPassword, string NewPassword) : IRequest<Unit>;
}
