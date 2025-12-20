using SocietyLogs.Application.DTOs.UserDTOs;
using SocietyLogs.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.Common.Interfaces
{
    public interface IUserService
    {
        // Kullanıcıyı ID ile bulup güncelleme işlemi
        Task<AppUser?> GetUserByIdAsync(Guid userId);

        // Tertemiz imza: ID ve DTO
        Task<bool> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDto userDto);
    }
}
