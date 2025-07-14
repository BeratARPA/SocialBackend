using MediatR;

namespace IdentityService.Application.Commands.TokenRefresh
{
    public record RefreshTokenCommand(string Token) : IRequest<RefreshTokenResponse>;

    public record RefreshTokenResponse(string AccessToken, string RefreshToken);
}
