using IdentityService.Application.Interfaces;
using IdentityService.Domain.Aggregates;
using MediatR;

namespace IdentityService.Application.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand,Unit>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ResetPasswordCommandHandler(
            IGenericRepository<User> userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                throw new Exception("User not found");

            // Burada normalde token doğrulaması yapılır (tabloya yazılmış olmalı).
            var newHash = _passwordHasher.HashPassword(request.NewPassword);
            user.SetPassword(newHash, "");

            await _userRepository.UnitOfWork.SaveEntitiesAsync();
            return Unit.Value;
        }
    }
}
