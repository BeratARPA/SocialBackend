using IdentityService.Application.Interfaces;
using IdentityService.Domain.Aggregates;
using MediatR;

namespace IdentityService.Application.Commands.ForgotPassword
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand,Unit>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordHandler(
            IGenericRepository<User> userRepository,
            IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return Unit.Value; // E-posta bilinmiyorsa bile saldırı riskini azaltmak için sessizce bitir.

            var resetToken = Guid.NewGuid().ToString();

            // Normalde token bir tabloya kaydedilir, burada stub:
            var resetUrl = $"https://yourapp.com/reset-password?token={resetToken}&email={user.Email}";

            await _emailSender.SendEmailAsync(user.Email, "Reset Password",
                $"Click here to reset your password: {resetUrl}");

            return Unit.Value;
        }
    }
}
