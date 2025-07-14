using IdentityService.Application.Interfaces;
using IdentityService.Domain.Aggregates;
using MediatR;

namespace IdentityService.Application.Commands.TokenRefresh
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RefreshTokenCommandHandler(
            IGenericRepository<RefreshToken> refreshTokenRepository,
            IGenericRepository<User> userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var oldToken = await _refreshTokenRepository.FirstOrDefaultAsync(r => r.Token == request.Token);
            if (oldToken == null || oldToken.IsExpired || !oldToken.IsActive)
                throw new Exception("Invalid or expired refresh token");

            var user = await _userRepository.GetByIdAsync(oldToken.UserId);
            if (user == null)
                throw new Exception("User not found");

            var newAccessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);
            var newRefreshToken = new RefreshToken(user.Id, Guid.NewGuid().ToString(), DateTime.UtcNow.AddDays(7), "127.0.0.1");

            oldToken.Revoke(newRefreshToken.Token);

            await _refreshTokenRepository.AddAsync(newRefreshToken);
            await _refreshTokenRepository.UnitOfWork.SaveEntitiesAsync();

            return new RefreshTokenResponse(newAccessToken, newRefreshToken.Token);
        }
    }

}
