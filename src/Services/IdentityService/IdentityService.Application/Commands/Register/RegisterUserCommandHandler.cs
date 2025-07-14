using IdentityService.Application.Interfaces;
using IdentityService.Domain.Aggregates;
using MediatR;

namespace IdentityService.Application.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterUserCommandHandler(
            IGenericRepository<User> userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                throw new Exception("User already exists");

            var hashedPassword = _passwordHasher.HashPassword(request.Password);
            var user = new User(request.Email, hashedPassword, ""); // Salt yoksa boş bırak.
            await _userRepository.AddAsync(user);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);

            return new RegisterUserCommandResponse(user.Id.ToString(), user.Email, token);
        }
    }
}
