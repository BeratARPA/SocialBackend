using IdentityService.Application.Interfaces;
using IdentityService.Domain.Aggregates;
using MediatR;

namespace IdentityService.Application.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Unit>
    {
        private readonly IGenericRepository<User> _userRepository;

        public ConfirmEmailCommandHandler(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null) throw new Exception("User not found");

            user.ConfirmEmail();
            await _userRepository.UnitOfWork.SaveEntitiesAsync();

            return Unit.Value;
        }
    }
}
