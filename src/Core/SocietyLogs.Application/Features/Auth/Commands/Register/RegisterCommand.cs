using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<Unit> // Geriye bir veri dönmeyeceğiz (void)
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
