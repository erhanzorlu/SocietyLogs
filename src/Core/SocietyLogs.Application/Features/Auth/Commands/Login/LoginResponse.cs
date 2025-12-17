using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Auth.Commands.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Email { get; set; }
    }
}
