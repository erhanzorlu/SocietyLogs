using Microsoft.AspNetCore.Identity;
using SocietyLogs.Domain.Common;
using SocietyLogs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocietyLogs.Domain.Entities.Identity
{
    // IdentityUser<Guid>: ID'nin Guid olacağını belirtiyoruz (Varsayılan string'dir).
    public class AppUser : IdentityUser<Guid>, IEntity, ISoftDeletable
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        // --- Profil ---
        public string? Title { get; set; }
        public string ProfileUrl { get; set; } = "default_profile.png";
        public string BannerUrl { get; set; } = "default_banner.png";
        public string? Bio { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderEnum Gender { get; set; }
        public bool IsVerified { get; set; }

        // --- Oyunlaştırma ---
        public int CurrentLevel { get; set; } = 1;
        public int TotalPoints { get; set; } = 0; // Cache amaçlı toplam puan
        public int ProfileCompletionRate { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        // --- Navigation Properties ---
        public ICollection<UserSocialMedia> SocialMedias { get; set; }
        public ICollection<UserEducation> Educations { get; set; }
        public ICollection<UserExperience> Experiences { get; set; }
        // UserBadge, Sertifikalar ve Puan Geçmişi aşağıda

        // --- BaseEntity Manuel Implementasyon ---
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }

        public void Delete() { IsDeleted = true; DeletedDate = DateTime.UtcNow; }
        public void UndoDelete() { IsDeleted = false; DeletedDate = null; }
    }

}
