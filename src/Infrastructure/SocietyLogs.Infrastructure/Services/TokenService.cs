using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocietyLogs.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(AppUser user, IList<string> roles)
        {
            // 1. Token içinde saklanacak bilgiler (Claims)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Kullanıcı ID
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),      // Email
                new Claim(ClaimTypes.Name, user.UserName!),                 // Kullanıcı Adı
                new Claim("Name", user.Name),                               // Adı
                new Claim("Surname", user.Surname)                          // Soyadı
            };

            // Rolleri ekle (Admin, User vs.)
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 2. İmzalama Anahtarı (AppSettings'den okuyoruz)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. Token Ayarları
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:DurationInMinutes"]!)),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = creds
            };

            // 4. Token Oluştur ve Döndür
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
