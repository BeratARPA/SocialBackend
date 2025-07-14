using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Application.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<Unit>;
}
