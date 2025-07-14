using IdentityService.Application.Interfaces;
using IdentityService.Domain.Aggregates;
using MediatR;

namespace IdentityService.Application.Queries.GetProfile
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
    {
        private readonly IGenericRepository<User> _userRepository;

        public GetProfileQueryHandler(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("User not found");

            return new UserProfileDto(user.Id.ToString(), user.Email, user.EmailConfirmed);
        }
    }
}
