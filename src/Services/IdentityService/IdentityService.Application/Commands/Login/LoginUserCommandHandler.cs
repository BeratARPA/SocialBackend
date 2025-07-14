using IdentityService.Application.Interfaces;
using IdentityService.Domain.Aggregates;
using MediatR;

namespace IdentityService.Application.Commands.Login
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginUserCommandHandler(
            IGenericRepository<User> userRepository,
            IGenericRepository<RefreshToken> refreshTokenRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var accessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);

            var refreshToken = new RefreshToken(user.Id, Guid.NewGuid().ToString(), DateTime.UtcNow.AddDays(7), "127.0.0.1");
            await _refreshTokenRepository.AddAsync(refreshToken);
            await _refreshTokenRepository.UnitOfWork.SaveEntitiesAsync();

            return new LoginUserCommandResponse(accessToken, refreshToken.Token);
        }
    }
}
