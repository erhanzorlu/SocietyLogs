using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Application.DTOs.UserDTOs
{
    public class UpdateUserProfileDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderEnum Gender { get; set; }

        // Servis dosya yüklemez, sadece yolu (string) kaydeder.
        public string? ProfileUrl { get; set; }
        public string? BannerUrl { get; set; }
    }
}
