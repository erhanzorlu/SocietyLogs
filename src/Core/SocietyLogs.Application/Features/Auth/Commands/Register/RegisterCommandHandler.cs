using MediatR;
using Microsoft.AspNetCore.Identity;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocietyLogs.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Unit>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // 1. Şifreler eşleşiyor mu? (Bunu Validator'a da koyabilirdik ama burada da olsun)
            if (request.Password != request.ConfirmPassword)
                throw new ValidationException("Şifreler eşleşmiyor!");

            // 2. Kullanıcıyı Oluştur
            var newUser = new AppUser
            {
                UserName = request.Email, // Identity UserName zorunlu tutar, Email'i kullanıyoruz
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname,
                SecurityStamp = Guid.NewGuid().ToString() // Token güvenliği için kritik
            };

            // 3. Identity Kütüphanesi ile Kaydet (Şifreyi otomatik hashlenecek)
            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                // Identity hatalarını toplayıp fırlatıyoruz
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(string.Join(", ", errors));
                // Not: ValidationException fırlattığımız için GlobalHandler bunu yakalayıp 400 dönecek.
            }

            return Unit.Value;
        }
    }
}
