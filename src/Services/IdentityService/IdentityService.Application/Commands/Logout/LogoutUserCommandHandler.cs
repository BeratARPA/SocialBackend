using IdentityService.Application.Interfaces;
using IdentityService.Domain.Aggregates;
using MediatR;

namespace IdentityService.Application.Commands.Logout
{
    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand,Unit>
    {
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;

        public LogoutUserCommandHandler(IGenericRepository<RefreshToken> refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Unit> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            var token = await _refreshTokenRepository.FirstOrDefaultAsync(r => r.Token == request.RefreshToken);
            if (token == null) return Unit.Value;

            token.Revoke("UserLogout");
            await _refreshTokenRepository.UnitOfWork.SaveEntitiesAsync();
            return Unit.Value;
        }      
    }
}
