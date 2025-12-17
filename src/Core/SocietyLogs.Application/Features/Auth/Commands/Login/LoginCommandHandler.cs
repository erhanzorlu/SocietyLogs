using MediatR;
using Microsoft.AspNetCore.Identity;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcıyı Bul
            var user = await _userManager.FindByEmailAsync(request.Email);

            // 2. Kullanıcı yoksa veya şifre yanlışsa (Güvenlik için genel hata dönülür)
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new Exception("Email veya şifre hatalı!"); // Global Handler bunu 500 veya 400'e çevirir
            }

            // 3. Kullanıcının Rollerini Al (Token'a koymak için)
            var roles = await _userManager.GetRolesAsync(user);

            // 4. Token Üret
            var token = _tokenService.CreateToken(user, roles);

            // 5. Cevabı Dön
            return new LoginResponse
            {
                Token = token,
                Email = user.Email,
                Expiration = DateTime.UtcNow.AddMinutes(60) // AppSettings ile uyumlu olmalı
            };
        }
    }
}
