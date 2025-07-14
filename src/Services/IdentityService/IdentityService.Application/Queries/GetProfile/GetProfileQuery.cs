using MediatR;

namespace IdentityService.Application.Queries.GetProfile
{
    public record GetProfileQuery(Guid UserId) : IRequest<UserProfileDto>;

    public record UserProfileDto(string UserId, string Email, bool EmailConfirmed);
}
