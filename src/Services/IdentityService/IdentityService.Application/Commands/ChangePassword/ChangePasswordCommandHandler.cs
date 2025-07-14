using IdentityService.Application.Interfaces;
using IdentityService.Domain.Aggregates;
using MediatR;

namespace IdentityService.Application.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ChangePasswordCommandHandler(
            IGenericRepository<User> userRepository,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("User not found");

            if (!_passwordHasher.VerifyPassword(request.OldPassword, user.PasswordHash))
                throw new Exception("Invalid old password");

            var newHash = _passwordHasher.HashPassword(request.NewPassword);
            user.SetPassword(newHash, "");

            await _userRepository.UnitOfWork.SaveEntitiesAsync();
            return Unit.Value;
        }
    }
}
