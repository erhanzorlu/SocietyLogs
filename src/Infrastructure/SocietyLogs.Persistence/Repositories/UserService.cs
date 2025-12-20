using Microsoft.AspNetCore.Identity;
using SocietyLogs.Application.Common.Interfaces;
using SocietyLogs.Application.DTOs.UserDTOs;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Persistence.Repositories
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser?> GetUserByIdAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<bool> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            // --- MAPPING (DTO -> ENTITY) ---
            // Elle atama Identity için en güvenlisidir ama Mapster da kullanılabilir.
            // Biz kontrolü elde tutmak için kritik alanları elle, diğerlerini otomatik yapabiliriz.

            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Title = dto.Title;
            user.Bio = dto.Bio;
            user.BirthDate = dto.BirthDate;
            user.Gender = dto.Gender;

            // Resim yolları sadece doluysa güncellenir
            if (!string.IsNullOrEmpty(dto.ProfileUrl)) user.ProfileUrl = dto.ProfileUrl;
            if (!string.IsNullOrEmpty(dto.BannerUrl)) user.BannerUrl = dto.BannerUrl;

            user.UpdatedDate = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
