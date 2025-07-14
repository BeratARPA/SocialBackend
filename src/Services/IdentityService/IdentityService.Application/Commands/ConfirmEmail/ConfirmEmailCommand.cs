using MediatR;

namespace IdentityService.Application.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(Guid UserId, string Token) : IRequest<Unit>;
}
